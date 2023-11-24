using SocialMedia.DTO;
using SocialMedia.Models;

namespace SocialMedia.Contracts
{
    public interface IPostRepository
    {
        Task<Post> CreateNewPost(Post post);
        Task<List<PostDto>>GetUserPosts(string userId,string profileId);
        Task<Post>EditPost(Guid id, Post post);
        Task<Post>DeletePost(Guid id);
        Task<List<PostDto>>GetUserFeed(string userId);
        Task<PostLikes> AddOrRemoveLikeToPost(PostLikes postLikes);
    }
}
