using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly ITokenService _tokenService;
        public readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginRequest.Username.ToLower());
                if (user == null)
                    return Unauthorized("Invalid username or password");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized("Invalid username or password");

                }
                return Ok(
                        new NewUserResponse
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        }
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new AppUser
                {
                    UserName = registerRequest.Username,
                    Email = registerRequest.Email
                };

                var createdUser = await _userManager.CreateAsync(user, registerRequest.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
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
                    else
                    {
                        return StatusCode(500, "Failed to add user to role");
                    }
                }
                else
                {
                    return StatusCode(500, "Failed to create user");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}