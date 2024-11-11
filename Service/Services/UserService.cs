using DataAccess.Models;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.Enums;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetListUserByRole(AppEnums.ROLE_ENUMS role)
        {
            int roleId = (int)role;

            return await unitOfWork.Users.GetAll(u => u.RoleId == roleId, u => u.Include(u => u.Role));
        }
    }
}
