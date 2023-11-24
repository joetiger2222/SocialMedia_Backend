using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string RecieverId { get; set; }

        [ForeignKey(nameof(User))]
        public string SenderId { get; set; }
        public User User { get; set; }
    }
}
