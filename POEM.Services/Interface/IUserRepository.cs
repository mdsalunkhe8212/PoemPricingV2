using POEM.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEM.Services.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> ValidateUserAsync(string email, string password);
        Task<bool> CreateUserAsync(User user);
    }
}
