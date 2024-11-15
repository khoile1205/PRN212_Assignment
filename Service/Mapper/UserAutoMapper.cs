using AutoMapper;
using DataAccess.Models;
using Service.DTO;
using Service.DTO.OrderDto;

namespace Service.Mapper
{
    public class UserAutoMapper : Profile
    {
        public UserAutoMapper()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<SignUpDto, User>().ReverseMap();
            CreateMap<OrderProductDto, OrderProduct>().ReverseMap();

        }
    }
}
