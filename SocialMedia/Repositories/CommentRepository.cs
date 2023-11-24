using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialMedia.Contracts;
using SocialMedia.Data;
using SocialMedia.DTO;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CommentRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<CommentDto> CreateNewComment(Guid postId, Comment comment)
        {
            if (comment.File != null)
            {
                var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{comment.FileName}{comment.FileExtention}");
                using var stream = new FileStream(localFilePath, FileMode.Create);
                await comment.File.CopyToAsync(stream);
                var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{comment.FileName}{comment.FileExtention}";
                comment.FilePath = urlFilePath;
            }
            
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
            var user = await dbContext.Users.FirstOrDefaultAsync(c => c.Id == comment.UserId);
            
            var commentDto = new CommentDto()
            {
                Id=comment.Id,
                UserId=comment.UserId,
                FullName=user.FirstName+" "+user.LastName,
                Text=comment.Text,
                Likes=comment.Likes,
                PostId=comment.PostId,
                FilePath = comment.FilePath,

            };
            
            return commentDto;
        }

        public async Task<Comment> DeleteComment(Guid commentId)
        {
           var comment=await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if(comment is null)
            {
                return null;
            }
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> EditComment(Guid CommentId, Comment comment)
        {
            var commentToEdit=await dbContext.Comments.FirstOrDefaultAsync(x=>x.Id == CommentId);
            if(commentToEdit is null)
            {
                return null;
            }
            commentToEdit.Text= comment.Text;
            commentToEdit.Likes= comment.Likes;
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<List<CommentDto>> GetAllComments(Guid postId)
        {
            var comments = await dbContext.Comments.Where(x => x.PostId == postId).Include(u => u.User).Select(c=>new CommentDto
            {
                Id = c.Id,
                FilePath=c.FilePath,
                Likes=c.Likes,
                PostId=postId,
                Text=c.Text,
                UserId=c.UserId,
                FullName=c.User.FirstName + " "+c.User.LastName,
            }).ToListAsync();
            return comments;
        }
    }
}
