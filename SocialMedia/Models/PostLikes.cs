using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class PostLikes
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
       

    }
}
