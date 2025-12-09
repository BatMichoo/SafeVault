using Microsoft.Data.SqlClient;

namespace SafeVault;

public class UserDataHandler
{
    private readonly string _connectionString = "Your_Secure_Connection_String";

    /// <summary>
    /// Retrieves a user's information securely using a parameterized query.
    /// </summary>
    /// <param name="username">The username provided by the user.</param>
    /// <returns>A SqlDataReader containing the user data, or null.</returns>
    public SqlDataReader GetUserByUsername(string username)
    {
        // SQL Statement uses a placeholder (@Username) instead of concatenating the input string.
        string sql = "SELECT UserID, Username, Email FROM Users WHERE Username = @Username;";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                // The key step: Add the user input as a parameter. 
                // The data is passed to the database engine separately from the query text.
                command.Parameters.AddWithValue("@Username", username);

                // ExecuteReader returns the data. 
                // In a real app, you would handle closing the reader/connection.
                return command.ExecuteReader(); 
            }
        }
    }
}
