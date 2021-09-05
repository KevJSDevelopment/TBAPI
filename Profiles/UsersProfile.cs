using AutoMapper;
using TwitterBattlesAPI.Dtos;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<User, UserCreateDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        }
    }
}