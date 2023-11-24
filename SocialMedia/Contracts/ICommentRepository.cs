using SocialMedia.DTO;
using SocialMedia.Models;

namespace SocialMedia.Contracts
{
    public interface ICommentRepository
    {
        Task<CommentDto> CreateNewComment(Guid postId, Comment comment);
        Task<List<CommentDto>> GetAllComments(Guid postId);
        Task<Comment> EditComment(Guid CommentId, Comment comment);
        Task<Comment> DeleteComment(Guid commentId);
    }
}
