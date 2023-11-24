using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; } = 0;


        [NotMapped]
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? FileExtention { get; set; }


        public string UserId { get; set; }
        public User User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
