using AutoMapper;
using DataAccess.Models;
using DataAccess.Repository;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
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
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            return _mapper.Map<UserDto>(account);

        }
    }
}
