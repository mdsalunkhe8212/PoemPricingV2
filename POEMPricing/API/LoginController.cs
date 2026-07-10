using POEM.Model.Model;
using POEM.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using POEM.Model;
using System.Web.Security;
using System.Web;
using POEMPricing.Managers;


namespace POEMPricing.API
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private readonly UserRepository _userRepository;
        public LoginController()
        {
            _userRepository = new UserRepository();
        }
        // -------------------------
        // LOGIN
        // -------------------------
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginRequestDto request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var isValid = await _userRepository.ValidateUserAsync(request.Email, request.Password);

            if (!isValid)
                return Unauthorized();

            // Issue FormsAuth cookie
            FormsAuthentication.SetAuthCookie(request.Email, false);


            return Ok(new { message = "Login successful" });
        }
        // -------------------------
        // LOGOUT
        // -------------------------
        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            // Clear FormsAuth cookie
            FormsAuthentication.SignOut();

            // Optionally clear session if used
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }

            return Ok(new { message = "Logout successful" });
        }
        // -------------------------
        // FORGOT PASSWORD
        // -------------------------
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            // Optional: check if user exists
            var user = await _userRepository.GetByEmailAsync(request.Email);

            // Always return OK for security
            return Ok(new { message = "If this email exists, a reset link has been sent." });
        }

        // -------------------------
        // CREATE USER
        // -------------------------
        [HttpPost]
        [Route("create-user")]
        public async Task<IHttpActionResult> CreateUser(CreateUserRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest("Email, full name, and password are required.");
            }

            var existing = await _userRepository.GetByEmailAsync(request.Email);
            if (existing != null)
                return Content(System.Net.HttpStatusCode.Conflict, "User already exists.");

            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                PasswordHash = request.Password, // will be hashed in repo
                RoleId = request.RoleId,
                IsActive = request.IsActive
            };

            await _userRepository.CreateUserAsync(user);

            return Ok(new { message = "User created successfully." });
        }

        // below is for usermanagement methods new
        // GET: api/users?pageNumber=1&pageSize=10&name=john&email=gmail
        [HttpGet]
        [Route("get-users")]
        public async Task<IHttpActionResult> GetUsers(
            int pageNumber = 1,
            int pageSize = 10,
            string name = null,
            string email = null)
        {
            var users = await _userRepository.GetUsersAsync(pageNumber, pageSize, name, email);
            var totalCount = await _userRepository.GetTotalUsersCountAsync();

            return Ok(new
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        // GET: api/users/5
        [HttpGet]
        [Route("get-user/{id:int}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/users/count
        [HttpGet]
        [Route("get-all-users-count")]
        public async Task<IHttpActionResult> GetTotalUsers()
        {
            var count = await _userRepository.GetTotalUsersCountAsync();

            return Ok(count);
        }

        // PUT: api/users/5
        [HttpPut]
        [Route("update-user/{id:int}")]
        public async Task<IHttpActionResult> UpdateUser(int id, User user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            if (id != user.LoginId)
                return BadRequest("User Id mismatch.");

            var updated = await _userRepository.UpdateUserAsync(user);

            if (!updated)
                return NotFound();

            return Ok(new
            {
                Success = true,
                Message = "User updated successfully."
            });
        }

        // DELETE: api/users/5
        [HttpDelete]
        [Route("delete-user/{id:int}")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            var deleted = await _userRepository.DeleteUserAsync(id);

            if (!deleted)
                return NotFound();

            return Ok(new
            {
                Success = true,
                Message = "User deleted successfully."
            });
        }

        [HttpGet]
        [Route("get-roles")]
        public async Task<IHttpActionResult> GetRoles()
        {
            var roles = await _userRepository.GetAllRolesAsync();

            return Ok(roles);
        }

        [HttpGet]
        [Route("test-email")]
        public async Task<IHttpActionResult> TestEmail()
        {
            try
            {
                var manager = new EmailManager();

                await manager.SendEmailAsync(
                    "test@gmail.com",
                    "POEM SMTP Test",
                    "<h2>Email sent successfully.</h2>");

                return Ok("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("send-reset-code")]
        public async Task<IHttpActionResult> SendResetCode([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest("Email is required.");

                email = email.Trim();

                var user = await _userRepository.GetActiveUserByEmailAsync(email);

                // 
                if (user == null)
                {
                    return Content(
                        System.Net.HttpStatusCode.NotFound,
                        "Email is not registered.");
                }

                var resetManager = new PasswordResetManager();

                string code = resetManager.GenerateCode();

                resetManager.SaveCode(email, code);

                var emailManager = new EmailManager();

                string body = $@"
            <p>Hello {user.FullName},</p>

            <p>Your password reset verification code is:</p>

            <h2>{code}</h2>

            <p>This code is valid for 10 minutes.</p>

            <br/>

            <p>Regards,<br/>POEM Pricing Team</p>";

                await emailManager.SendEmailAsync(
                    email,
                    "Password Reset Verification Code",
                    body);

                return Ok(new
                {
                    Success = true,
                    Message = "If the email is registered, a verification code has been sent."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request.");

                if (string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Code) ||
                    string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.ConfirmPassword))
                {
                    return BadRequest("All fields are required.");
                }

                if (request.Password != request.ConfirmPassword)
                {
                    return BadRequest("Password and Confirm Password do not match.");
                }

                var passwordRegex = new System.Text.RegularExpressions.Regex(
                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()_\-+=]).{8,}$");

                if (!passwordRegex.IsMatch(request.Password))
                {
                    return BadRequest(
                        "Password must be at least 8 characters long and contain:\n\n" +
                        "• One uppercase letter\n" +
                        "• One lowercase letter\n" +
                        "• One number\n" +
                        "• One special character");
                }

                var resetManager = new PasswordResetManager();

                if (!resetManager.ValidateCode(request.Email, request.Code))
                {
                    return BadRequest("Invalid or expired verification code.");
                }

                var updated = await _userRepository.UpdatePasswordAsync(
                    request.Email,
                    request.Password);

                if (!updated)
                {
                    return BadRequest("Unable to update password.");
                }

                resetManager.RemoveCode(request.Email);

                return Ok(new
                {
                    Success = true,
                    Message = "Password updated successfully."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}