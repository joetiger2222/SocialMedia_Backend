using Microsoft.EntityFrameworkCore;
using SocialMedia.Contracts;
using SocialMedia.Data;
using SocialMedia.DTO;
using SocialMedia.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SocialMedia.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PostRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Post> CreateNewPost(Post post)
        {
            if(post.File!= null)
            {
                var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{post.FileName}{post.FileExtention}");
                using var stream = new FileStream(localFilePath, FileMode.Create);
                await post.File.CopyToAsync(stream);
                var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{post.FileName}{post.FileExtention}";
                post.FilePath = urlFilePath;
            }
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return post;

        }

        public async Task<Post> DeletePost(Guid id)
        {
            var postToDelete=await dbContext.Posts.FirstOrDefaultAsync(post => post.Id == id);
            if(postToDelete is null)
            {
                return null;
            }
            dbContext.Posts.Remove(postToDelete);
            await dbContext.SaveChangesAsync();
            return postToDelete;
        }

        public async Task<Post> EditPost(Guid id, Post post)
        {
            var postToEdit=await dbContext.Posts.FirstOrDefaultAsync(post => post.Id == id);
            if (postToEdit == null)
            {
                return null;
            }
            postToEdit.Text = post.Text;
            postToEdit.Likes= post.Likes;
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<List<PostDto>> GetUserPosts(string userId,string profileId)
        {
            //var user=await dbContext.Users.FirstOrDefaultAsync(x=>x.Id==userId);
            //var posts = await dbContext.Posts.Where(x => x.UserId == userId).ToListAsync();
            //foreach (var p in posts)
            //{
            //    var isLiked = await dbContext.PostLikes.FirstOrDefaultAsync(x => x.UserId == userId && p.Id == x.PostId);
            //    if (isLiked != null)
            //    {
            //        p.IsLiked = true;
            //    }
            //}
            //return posts;
            var posts = await dbContext.Posts.Where(x => x.UserId == profileId).Include(u => u.User).Select(x => new PostDto
            {
                FilePath = x.FilePath,
                Id = x.Id,
                Likes = x.Likes,
                Text = x.Text,
                UserId = x.UserId,
                FullName = x.User.FirstName + " " + x.User.LastName,
                IsLiked = dbContext.PostLikes.Any(p => p.UserId == userId && p.PostId == x.Id)
            }).ToListAsync();
            return posts;
        }

        public async Task<List<PostDto>> GetUserFeed(string userId)
        {
            //var posts = await dbContext.Posts.Include("User").Where(p => p.UserId != userId).ToListAsync();
            //foreach (var p in posts)
            //{
            //    var isLiked = await dbContext.PostLikes.FirstOrDefaultAsync(x => x.UserId == userId && p.Id == x.PostId);
            //    if (isLiked != null)
            //    {
            //        p.IsLiked = true;
            //    }
            //}
            //return posts;
            var posts = await dbContext.Posts.Include(u=>u.User).Select(x => new PostDto
            {
                FilePath=x.FilePath,
                Id=x.Id,
                Likes=x.Likes,
                Text=x.Text,
                UserId=x.UserId,
                FullName=x.User.FirstName+" "+x.User.LastName,
                IsLiked=dbContext.PostLikes.Any(p=>p.UserId==userId&&p.PostId==x.Id)
            }).ToListAsync();
            return posts;
        }

        public async Task<PostLikes> AddOrRemoveLikeToPost(PostLikes postLikes)
        {
            var like = await dbContext.PostLikes.FirstOrDefaultAsync(x => x.PostId == postLikes.PostId && x.UserId == postLikes.UserId);
            if(like is null)
            {
                await dbContext.PostLikes.AddAsync(postLikes);
                await dbContext.SaveChangesAsync();
                var post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postLikes.PostId);
                post.Likes++;
                await dbContext.SaveChangesAsync();
                return postLikes;
            }
            dbContext.PostLikes.Remove(like);
            await dbContext.SaveChangesAsync();
            var postt = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postLikes.PostId);
            postt.Likes--;
            await dbContext.SaveChangesAsync();
            return postLikes;
           
        }
    }
}
