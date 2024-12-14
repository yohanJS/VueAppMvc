using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

/// <summary>
/// Now that roles exist and users can be assigned roles, 
/// you can protect specific endpoints to ensure only users 
/// with the Admin role can access them.
/// Add[Authorize(Roles = "Admin")] Attribute
/// </summary>
[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok("This is an admin-only endpoint.");
    }
}

/*
 How This Works:
The [Authorize(Roles = "Admin")] attribute checks if the authenticated user has the Admin role.
If the user does not have the Admin role, they will receive a 403 Forbidden response.
If the user has the correct role, they can access the endpoint.
 */
