using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Models;

namespace SafeVault.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserDataHandler _userDataHandler;

    public HomeController(ILogger<HomeController> logger, UserDataHandler handler)
    {
        _userDataHandler = handler;
        _logger = logger;
    }

    [HttpPost]
    [Route("/submit")] // Matches the form action="/submit"
    public IActionResult Submit(string username, string email)
    {
        // 1. Input Sanitization and Validation (from Step 2)

        // Sanitize inputs
        string sanitizedUsername = InputSanitizer.SanitizeInput(username);
        string sanitizedEmail = InputSanitizer.SanitizeInput(email);

        // Basic validation (e.g., check for email format)
        if (!InputSanitizer.IsValidEmail(sanitizedEmail))
        {
            // Return an error if validation fails
            ViewBag.ErrorMessage = "Invalid email format.";
            return View("Error"); // Assuming an Error view exists
        }

        // 2. Database Interaction (Using Parameterized Query from Step 3)
        // This example shows an insertion, but the principle is the same.
        // If the goal was to log in, you'd use GetUserByUsername.

        try
        {
            // Placeholder for an actual insertion or retrieval using the handler
            // For insertion, you'd need an AddUser method in UserDataHandler
            // Example of retrieval:
            var userReader = _userDataHandler.GetUserByUsername(sanitizedUsername);

            if (userReader.HasRows)
            {
                ViewBag.Message = $"User '{sanitizedUsername}' found successfully.";
                // Process the data...
            }
            else
            {
                ViewBag.Message = $"User '{sanitizedUsername}' not found.";
                // In a real application, you might insert the new user here.
            }

            userReader.Close(); // Close the reader after use

            return View("Success"); // Redirect or show a success view
        }
        catch (System.Exception ex)
        {
            // Log the exception
            ViewBag.ErrorMessage = "An error occurred during data processing.";
            return View("Error");
        }
    }

    public IActionResult Index(string username, string email)
    {
        // 1. Input Sanitization and Validation (from Step 2)

        // Sanitize inputs
        string sanitizedUsername = InputSanitizer.SanitizeInput(username);
        string sanitizedEmail = InputSanitizer.SanitizeInput(email);

        // Basic validation (e.g., check for email format)
        if (!InputSanitizer.IsValidEmail(sanitizedEmail))
        {
            // Return an error if validation fails
            ViewBag.ErrorMessage = "Invalid email format.";
            return View("Error"); // Assuming an Error view exists
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
