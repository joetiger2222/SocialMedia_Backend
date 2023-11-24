using System.ComponentModel.DataAnnotations;

namespace SocialMedia.DTO
{
    public class RegisterUserDto
    {
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImgPath { get; set; }

        public string[] Roles { get; set; }
    }
}
