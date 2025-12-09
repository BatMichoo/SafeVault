using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

[TestFixture]
public class TestSecurity
{
    // --- Authentication Tests ---

    [Test]
    public async Task TestInvalidLoginAttempt()
    {
        // Mock the SignInManager to simulate a failed password check
        var mockUserManager = new Mock<UserManager<ApplicationUser>>(/* ... constructor args ... */);
        var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(/* ... constructor args ... */);

        // Setup the mock to return a failed result when CheckPasswordSignInAsync is called
        mockSignInManager.Setup(x =>
            x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), "wrongpassword", It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Failed);

        var authService = new AuthenticationService(mockUserManager.Object, mockSignInManager.Object);

        // Act: Try to log in with an incorrect password
        bool result = await authService.AuthenticateUser("validUser", "wrongpassword");

        // Assert: Login attempt must fail
        Assert.That(result, Is.False, "Authentication should fail for an incorrect password.");
    }

    // NOTE: A successful login test would require setting up the mock to return SignInResult.Success.

    // --- Authorization Tests (Attribute Check) ---

    [Test]
    public void TestAdminDashboardIsProtectedByAdminRole()
    {
        // Use Reflection to check the attributes on the AdminDashboard method
        var method = typeof(AdminController).GetMethod(nameof(AdminController.AdminDashboard));

        // 1. Check for the [Authorize] attribute
        var authAttribute = method.GetCustomAttribute<AuthorizeAttribute>();
        Assert.That(authAttribute, Is.Not.Null, "AdminDashboard must have an [Authorize] attribute.");

        // 2. Check that the Roles property is set to 'Admin'
        Assert.That(authAttribute.Roles, Is.EqualTo("Admin"),
            "AdminDashboard must explicitly require the 'Admin' role.");
    }

    [Test]
    public void TestUserProfileRequiresAnyAuthentication()
    {
        // Use Reflection to check the attributes on the UserProfile method
        var method = typeof(StandardController).GetMethod(nameof(StandardController.UserProfile));

        // Check for the [Authorize] attribute
        var authAttribute = method.GetCustomAttribute<AuthorizeAttribute>();
        Assert.That(authAttribute, Is.Not.Null, "UserProfile must have an [Authorize] attribute.");

        // Check that the Roles property is null or empty (meaning any authenticated user is allowed)
        Assert.That(authAttribute.Roles, Is.Null.Or.Empty,
            "UserProfile should allow access to any authenticated user.");
    }
}
