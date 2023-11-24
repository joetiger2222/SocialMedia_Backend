namespace SocialMedia.DTO
{
    public class CreateCommentDto
    {
        
        public string UserId { get; set; }
        public string Text { get; set; }
        public Guid PostId { get; set; }
        public string? FileName { get; set; }
        public IFormFile? File { get; set; }
    }
}
