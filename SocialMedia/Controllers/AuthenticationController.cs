using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTO;
using SocialMedia.Models;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public AuthenticationController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserDto registerUserDto)
        {
            var identityUser = new User
            {
                UserName = registerUserDto.Username,
                Email = registerUserDto.Email,
                FirstName= registerUserDto.FirstName,
                LastName= registerUserDto.LastName,
                ImgPath= registerUserDto.ImgPath,
            };
            var result = await userManager.CreateAsync(identityUser, registerUserDto.Password);
            if (result.Succeeded)
            {
                if (registerUserDto.Roles != null && registerUserDto.Roles.Any())
                {
                    result = await userManager.AddToRolesAsync(identityUser, registerUserDto.Roles);
                    if (result.Succeeded)
                    {
                        return Ok("User Register Successfully");
                    }
                }

            }
            return BadRequest("Error Happened");
        }




        [HttpPost]
        [Route("Login/User")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user != null)
            {
                var checkPass = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPass)
                {
                    // Check the user's role
                    var userRoles = await userManager.GetRolesAsync(user);
                    if (userRoles.Contains("Reader"))
                    {
                        return Ok(new { UserId = user.Id, Role = "User" });
                    }

                }
            }
            return BadRequest();
        }






    }
}
