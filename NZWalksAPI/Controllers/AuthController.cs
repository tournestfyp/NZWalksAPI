using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public readonly ITokenRepository tokenRepository;


        // Identity provides us with userManager class to setup Registeration 
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(IdentityUser, registerRequestDto.Password);

            if (identityResult.Succeeded) 
            {
                // Now assign or add roles to the user bcz user is registered now
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any()) 
                {
                    identityResult = await userManager.AddToRolesAsync(IdentityUser, registerRequestDto.Roles);

                    if(identityResult.Succeeded) 
                    {
                        return Ok("User was registered! please login.");
                    }
                }
            }
            return BadRequest("Something Went Wrong!");
        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if(user !=null)
            {
                var chechkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if(chechkPasswordResult)
                {

                    // Token here to communicate (already have user, lets have the roles list using UserManager class
                    var roles = await userManager.GetRolesAsync(user);
                    if(roles !=null)
                    {
                        // create token for that user now

                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                    return Ok();
                }

            }
            return BadRequest("UserName or Password Is Incorrect");
        }
    }
}
