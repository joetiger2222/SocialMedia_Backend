using SocialMedia.Models;

namespace SocialMedia.DTO
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; } = 0;

        public string UserId { get; set; }
        public string FilePath { get; set; }
        public bool IsLiked { get; set; }
        public string FullName { get; set; }
        public User User { get; set; }
        
    }
    }
