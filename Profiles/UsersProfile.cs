using AutoMapper;
using TwitterBattlesAPI.Dtos;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<User, UserCreateDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<TweetCreateDto, Tweet>();
            CreateMap<TweetReadDto, Tweet>();
            CreateMap<UserLoginDto, UserReadDto>();
            CreateMap<WalletCreateDto, WalletAddress>();
            CreateMap<BookmarkCreateDto, Bookmark>();
            CreateMap<MessageCreateDto, Message>();
            CreateMap<FollowerCreateDto, Follower>();
        }
    }
}