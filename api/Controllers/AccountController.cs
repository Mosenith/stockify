using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try 
            {
                if(!ModelState.IsValid) 
                    return BadRequest(ModelState);

                var user = new AppUser
                {
                    UserName = registerRequest.Username,
                    Email = registerRequest.Email
                };
                
                var createdUser = await _userManager.CreateAsync(user, registerRequest.Password);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserResponse
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
                    }
                    else {
                        return StatusCode(500, "Failed to add user to role");
                    }
                }
                else {
                    return StatusCode(500, "Failed to create user");
                }
            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}