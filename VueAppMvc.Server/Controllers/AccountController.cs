using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
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
            string? connectionString = _iConfiguration.GetConnectionString("defaultConnectionString");
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
                    if (!string.IsNullOrEmpty(model.Email) && model.Email.Equals("yoanvaldes01@yahoo.es"))
                    {
                        await _userManager.AddToRoleAsync(user, "YohanJS");
                    }
                    // Generate email confirmation token
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Create confirmation link
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);

                    // Send confirmation email
                    try
                    {
                        SendEmail emailSender = new SendEmail();
                        var response = await emailSender.Execute(
                            model.Email,
                            "Confirm Your Email",
                            callbackUrl);

                        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            return Ok(new { message = "Registration successful. Please confirm your email." });
                        }
                        else
                        {
                            return StatusCode(500, "Error sending confirmation email.");
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Error sending email: {ex.Message}");
                    }
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // Check if userId or code is missing
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest(new { message = "Invalid email confirmation request." });
            }

            // Find the user by userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Decode the code
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // Confirm the email
            var result = await _userManager.ConfirmEmailAsync(user, code);

            // Return success or failure based on result
            if (result.Succeeded)
            {
                return Ok(new { message = "Email confirmed successfully. You can close this now. Thanks!" });
            }

            // Return failure message if confirmation fails
            return BadRequest(new { message = "Email confirmation failed." });
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
