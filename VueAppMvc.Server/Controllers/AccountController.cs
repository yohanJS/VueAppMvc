﻿using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _iConfiguration;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _iConfiguration = configBuilder;
        }

        [HttpGet("IsDev")]
        public IActionResult IsDev()
        {
            string? connectionString =  _iConfiguration.GetConnectionString("defaultConnectionString");
            if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("LAPTOP-GARCIA"))
            {
                return Ok("false");
            }
            return Ok("true");
        }

        /// <summary>
        /// Assign Role Manually via an Endpoint
        /// You can also create a dedicated endpoint for assigning roles to users:
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost("assign-role")]
        //public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null) return NotFound("User not found.");

        //    var result = await _userManager.AddToRoleAsync(user, model.Role);
        //    if (result.Succeeded) return Ok($"Role {model.Role} assigned to {model.Email}.");

        //    return BadRequest(result.Errors);
        //}

        //public class RoleAssignmentModel
        //{
        //    public string Email { get; set; }
        //    public string Role { get; set; }
        //}

        /// <summary>
        /// When a user registers with the email admin@example.com, 
        /// they are automatically assigned the Admin role.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            if (model != null && !string.IsNullOrEmpty(model.Password))
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the Admin role if the email matches a specific condition
                    if (model.Email == "yoanvaldes01@yahoo.es")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }

                    return Ok(new { message = "User registered successfully." });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Login successful." });
                }
            }
            return Unauthorized(new { message = "Invalid login attempt." });
        }
    }
}
