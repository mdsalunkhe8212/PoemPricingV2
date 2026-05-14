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


    }
}