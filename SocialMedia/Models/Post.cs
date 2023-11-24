using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocialMedia.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; } = 0;
        public bool? IsLiked { get; set; }

        public string UserId { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? FileExtention { get; set; }
       
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostLikes> PostLikes { get; set; }
    }
}
