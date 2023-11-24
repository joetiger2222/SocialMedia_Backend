using AutoMapper;
using SocialMedia.DTO;
using SocialMedia.Models;

namespace SocialMedia.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post,CreatePostDto>().ReverseMap();
            CreateMap<Post,PostDto>().ReverseMap();
            CreateMap<Post,EditPostDto>().ReverseMap();
            CreateMap<Comment,CreateCommentDto>().ReverseMap();
            CreateMap<Comment,CommentDto>().ReverseMap();
            CreateMap<Comment,EditCommentDto>().ReverseMap();
            CreateMap<FriendRequest,SendFriendRequestDto>().ReverseMap();
            CreateMap<PostLikes,AddLikeDto>().ReverseMap();
            CreateMap<Chat,MessageViewModel>().ReverseMap();
        }
    }
}
