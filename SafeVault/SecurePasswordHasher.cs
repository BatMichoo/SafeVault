namespace SafeVault;

public static class SecurePasswordHasher
{
    /// <summary>
    /// Hashes a plain-text password using BCrypt.
    /// BCrypt generates a salt internally and includes it in the hash output,
    /// making the hash self-contained for verification.
    /// </summary>
    /// <param name="password">The plain-text password.</param>
    /// <returns>The salted and hashed password string.</returns>
    public static string HashPassword(string password)
    {
        // Hash the password with a generated salt. 
        // The '10' is the work factor (cost). Higher is more secure but slower.
        return BCrypt.Net.BCrypt.HashPassword(password, 10);
    }

    /// <summary>
    /// Verifies a plain-text password against a stored BCrypt hash.
    /// </summary>
    /// <param name="password">The plain-text password provided by the user.</param>
    /// <param name="hashedPassword">The stored hashed password retrieved from the database.</param>
    /// <returns>True if the password matches the hash, otherwise False.</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // BCrypt extracts the salt from the stored hash and uses it to hash the
        // provided password, then compares the results. This is resistant to timing attacks.
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
