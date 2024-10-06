using AutoMapper;
using DataAccess.Models;
using Service.DTO;

namespace Service.Mapper
{
    public class UserAutoMapper : Profile
    {
        public UserAutoMapper()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
