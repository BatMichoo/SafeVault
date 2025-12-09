using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

// Define a simple User model
public class ApplicationUser : IdentityUser 
{
    // Custom properties can be added here
}

public class AuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Authenticates a user by checking credentials against the hashed password.
    /// </summary>
    /// <returns>True if authentication is successful.</returns>
    public async Task<bool> AuthenticateUser(string username, string password)
    {
        // 1. Find the user by username
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return false;
        }

        // 2. Verify the password against the securely hashed version in the database.
        // Identity handles the salted hashing and verification logic internally.
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // If successful, sign the user in (e.g., set the authentication cookie)
            await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }

        return false;
    }
}
