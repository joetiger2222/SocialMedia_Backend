namespace SocialMedia.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string? SenderId { get; set; }
        public string? RecieverId { get; set; }
    }
}
