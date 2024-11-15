using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstraction
{
    public interface IAuthService
    {
        Task<UserDto> login(LoginDto loginDto);

        Task<bool> signUp(SignUpDto signUpDto);
        void logOut();
    }
}
