using Org.BouncyCastle.Crypto.Generators;
using POEM.Model.Model;
using POEM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace POEM.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository()
        {
            _context  = new ApplicationDbContext(); 
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);

            if (user == null || !user.IsActive)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetUsersAsync(
           int pageNumber,
           int pageSize,
           string name = null,
           string email = null)
        {
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.FullName.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(x => x.Email.Contains(email));

            return await query
                .OrderBy(x => x.LoginId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<User> GetUserByIdAsync(int loginId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.LoginId == loginId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(x => x.LoginId == user.LoginId);

                if (existingUser == null)
                    return false;

                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.RoleId = user.RoleId;
                existingUser.IsActive = user.IsActive;

                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    existingUser.PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int loginId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.LoginId == loginId);

            if (user == null)
                return false;

            _context.Users.Remove(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserRoleDbDto>> GetAllRolesAsync()
        {
            return await _context.UserRoles
                .OrderBy(r => r.Role)
                .ToListAsync();
        }
        public async Task<User> GetActiveUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.IsActive);
        }

        public async Task<bool> UpdatePasswordAsync(string email, string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.IsActive);

            if (user == null)
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}