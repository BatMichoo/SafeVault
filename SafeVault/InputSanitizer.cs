using System.Text.RegularExpressions;

namespace SafeVault;

public static class InputSanitizer
{
    /// <summary>
    /// Sanitizes input strings to prevent XSS and basic SQL injection attempts.
    /// This is an initial defense; parameterized queries are the primary defense against SQLi.
    /// </summary>
    /// <param name="input">The raw user input string.</param>
    /// <returns>The sanitized string.</returns>
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // 1. HTML/XSS Sanitize: Remove potential script tags and event handlers.
        // This is a basic approach. For robust XSS, use an established library like AntiXSS.
        string sanitized = input.Replace("<", "&lt;").Replace(">", "&gt;");

        // 2. SQL Basic Sanitize: Remove common SQL injection indicators.
        // Primary SQLi defense is parameterized queries (Step 3).
        sanitized = Regex.Replace(sanitized, @"(['"";\-\\])", "", RegexOptions.IgnoreCase);

        // 3. Trim excessive whitespace and return.
        return sanitized.Trim();
    }

    /// <summary>
    /// Validates an email format.
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        // Simple regex for email validation (can be more complex)
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }
}
