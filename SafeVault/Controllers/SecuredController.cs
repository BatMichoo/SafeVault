using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Role names must match the roles assigned in the database/Identity system
public static class UserRoles
{
    public const string Admin = "Admin";
    public const string StandardUser = "User";
}

public class AdminController : Controller
{
    [HttpGet]
    [Route("dashboard")]
    // Authorization Attribute: Only users authenticated AND in the "Admin" role can access this.
    [Authorize(Roles = UserRoles.Admin)] 
    public IActionResult AdminDashboard()
    {
        return View(); // Returns the Admin Dashboard view
    }
    
    [HttpGet]
    [Route("tools")]
    // This route is accessible by both Admin and StandardUser.
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.StandardUser)] 
    public IActionResult SharedTools()
    {
        return View(); // Returns the Shared Tools view
    }
}

public class StandardController : Controller
{
    [HttpGet]
    [Route("profile")]
    // Requires any valid, authenticated user to access.
    [Authorize] 
    public IActionResult UserProfile()
    {
        return View();
    }
}
