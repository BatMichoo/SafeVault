using NUnit.Framework;
using SafeVault;

namespace UnitTests;

[TestFixture]
public class TestInputValidation
{
    // Assume InputSanitizer and UserDataHandler from Steps 2 and 3 exist in the scope.

    [Test]
    public void TestForSQLInjectionAttempt()
    {
        // Malicious input attempt to bypass authentication or drop the table.
        string maliciousInput = "' OR 1=1 --";

        // 1. Test Sanitization Function (Step 2)
        string sanitizedInput = InputSanitizer.SanitizeInput(maliciousInput);

        // Assert that critical SQL characters have been removed or escaped. 
        // The characters ', ", ;, -, \ are removed by the Step 2 sanitizer.
        Assert.That(sanitizedInput, Is.EqualTo("OR 1=1"));

        // 2. Test Parameterized Query (Step 3) - Conceptual Test
        // The parameterized query should treat " ' OR 1=1 -- " as a literal username, 
        // not as executable SQL. If the query runs and finds more than 0 users 
        // (assuming the user "' OR 1=1 --" doesn't exist), the test would fail.

        // NOTE: An actual integration test would require a mocked or real database connection 
        // to verify that only a single, non-existent user is looked up.
    }

    [Test]
    public void TestForXSSAttempt()
    {
        // Malicious input containing a script that attempts to run in the browser.
        string maliciousInput = "<script>alert('XSS Attack!');</script>";

        // Test Sanitization Function (Step 2)
        string sanitizedInput = InputSanitizer.SanitizeInput(maliciousInput);

        // Check if the script tags have been neutralized (e.g., converted to HTML entities).
        // The Step 2 sanitizer replaces < with &lt; and > with &gt;.
        Assert.That(sanitizedInput,
            Is.EqualTo("&lt;script&gt;alert('XSS Attack!');&lt;/script&gt;"),
            "The input should be neutralized to prevent browser execution.");

        // Check if the browser would interpret the neutralized input as a script.
        // It should be treated as plain text, not executable code.
        Assert.That(sanitizedInput, Does.Not.Contain("<script>"),
            "The output should not contain raw script tags.");
    }
}
