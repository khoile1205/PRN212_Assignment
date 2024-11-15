using AutoMapper;
using DataAccess.Models;
using DataAccess.Repository;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.Enums;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        private UserDto _currentUser;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public UserDto getCurrentUser()
        {
            return _currentUser;
        }
        public async Task<UserDto?> login(LoginDto loginDto)
        {

            User account = await _unitOfWork.Users
                .GetFirstOrDefault(predicate: u => u.Username == loginDto.Username && u.Password == loginDto.Password,
                                   include: u => u.Include(x => x.Role));

            if (account == null)
            {
                return null;
            }

            UserDto userData = _mapper.Map<UserDto>(account);
            _currentUser = userData;

            return userData;

        }

        public async Task<bool> signUp(SignUpDto signUpDto)
        {
            if (string.IsNullOrWhiteSpace(signUpDto.Username) || string.IsNullOrWhiteSpace(signUpDto.Password) || string.IsNullOrWhiteSpace(signUpDto.ConfirmPassword))
            {
                return false; // or throw an exception for invalid input
            }

            if (!signUpDto.Password.Equals(signUpDto.ConfirmPassword))
            {
                return false;
            }

            var existingUser = await _unitOfWork.Users.GetFirstOrDefault(u => u.Username == signUpDto.Username);
            if (existingUser != null)
            {
                return false; // User already exists
            }

            var roles = await _unitOfWork.Roles.GetFirstOrDefault(r => r.Id == (int)AppEnums.ROLE_ENUMS.Admin);
            var newUser = _mapper.Map<User>(signUpDto);

            newUser.Password = signUpDto.Password;
            newUser.RoleId = roles?.Id ?? 1;

            _unitOfWork.Users.Add(newUser);
            _unitOfWork.Save();

            return true;
        }

        public void logOut()
        {
            _currentUser = null;
        }
    }
}
