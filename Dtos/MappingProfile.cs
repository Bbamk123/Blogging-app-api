using AutoMapper;
using BloggingApi.Models;
using BloggingApi.Dtos;

namespace BloggingApi.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<User, UserDto>();
            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Follow, FollowDto>()
                .ForMember(dest => dest.FollowerName, opt => opt.MapFrom(src => src.Follower.UserName))
                .ForMember(dest => dest.FollowingName, opt => opt.MapFrom(src => src.Following.UserName));
        }
    }
}