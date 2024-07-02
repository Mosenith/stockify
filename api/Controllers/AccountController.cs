using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly UserManager<AppUser> _userMangaer;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userMangaer = userManager;
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
                
                var createdUser = await _userMangaer.CreateAsync(user, registerRequest.Password);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userMangaer.AddToRoleAsync(user, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok("User added successfully");
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