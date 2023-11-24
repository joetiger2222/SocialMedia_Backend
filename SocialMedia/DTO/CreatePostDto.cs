namespace SocialMedia.DTO
{
    public class CreatePostDto
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public string? FileName { get; set; }
        public IFormFile? File { get; set; }
    }
}
