using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            //Friends=new HashSet<User>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImgPath { get; set; }
        //public ICollection<User> Friends { get; set; }
        
    }
}
