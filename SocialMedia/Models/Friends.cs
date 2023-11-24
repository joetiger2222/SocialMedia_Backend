using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Friends
    {
        public int Id { get; set; }
        public string FirstId { get; set; }
        [ForeignKey("FirstId")]
        public User? First { get; set; }
        public string SecondId { get; set; }
        [ForeignKey("SecondId")]
        public User? Second { get; set; }

        
        
    }
}
