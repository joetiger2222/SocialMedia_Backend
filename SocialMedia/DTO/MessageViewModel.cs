namespace SocialMedia.DTO
{
    public class MessageViewModel
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string? SenderId { get; set; }
        public string? RecieverId { get; set; }
    }
}
