using SocialMedia.Models;

namespace SocialMedia.DTO
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        

        public string FullName { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; } = 0;
        public Guid PostId { get; set; }
        public string FilePath { get; set; }
        
    }
}
