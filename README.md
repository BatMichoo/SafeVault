# SafeVault

SafeVault is a secure web application designed to manage sensitive data, including user credentials and financial records.

1. SQL Injection Mitigation 🚫
   Vulnerability Identified: The initial risk was the reliance on raw user input (e.g., username) being concatenated directly into a SQL command string.

Fix Applied: The core fix was generating the secure database retrieval method using the Microsoft.Data.SqlClient.SqlCommand.Parameters.AddWithValue method (Step 3).

Code Reference (Illustrates the fix):

C#

The SQL query uses a placeholder, NOT direct concatenation.
string sql = "SELECT UserID, Username, Email FROM Users WHERE Username = @Username;";

The input is added as a parameter object.
command.Parameters.AddWithValue("@Username", username);
This approach ensures the database engine treats the user input strictly as data, neutralizing any embedded SQL commands (like ' OR 1=1 --).

2. Cross-Site Scripting (XSS) Mitigation 📝
   Vulnerability Identified: If a user submits input like <script>alert('xss');</script> and it is later rendered directly onto a web page, the malicious script would execute in other users' browsers.

Fix Applied: Generated the static InputSanitizer class (Step 2). This function handles the crucial defense-in-depth step by converting control characters before they reach the database or are processed by the application.

Code Reference (Illustrates the fix):

C#

public static string SanitizeInput(string input)
{
// Converts < and > to HTML entity representations
string sanitized = input.Replace("<", "&lt;").Replace(">", "&gt;");
// ... further SQL character stripping ...
return sanitized.Trim();
}
This ensures that when the data is eventually displayed, the browser interprets it as harmless plain text, not executable HTML/JavaScript.

Copilot summary

Vulnerability Type,Potential Location,Analysis (Step 2),Fix Applied (Step 3)
SQL Injection (SQLi),Database query execution logic within UserDataHandler.,Any direct string concatenation of user input into the SQL command text.,Replaced with Parameterized Queries (@Username placeholder) in the UserDataHandler.GetUserByUsername method.
Cross-Site Scripting (XSS),"Handling and display of raw user input (e.g., username, email) in views.",Lack of output encoding or sanitization before displaying inputs in a web page.,"Implemented the InputSanitizer.SanitizeInput function to convert malicious characters (<, >) into HTML entities (&lt;, &gt;)."
