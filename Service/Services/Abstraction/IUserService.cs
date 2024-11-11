using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Enums.AppEnums;

namespace Service.Services.Abstraction
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetListUserByRole(ROLE_ENUMS role);
        public Task<User> AddUser(User user);
        public Task<User> UpdateUser(User user);
        public Task<bool> DeleteUser(int userId);
        public Task<User?> GetUserById(int userId);
    }
}
