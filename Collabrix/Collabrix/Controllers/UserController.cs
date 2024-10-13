using Collabrix.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Collabrix.Controllers
{
    public class UserController
    {
        private static IConfiguration Configuration { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<List<User>> GetUsers()
        {
            List<User> Users = new List<User>();
            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("Default")))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Users";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetGuid(reader.GetOrdinal("user_id")), // Changed to "user_id"
                                    Username = reader.GetString(reader.GetOrdinal("username")), // Changed to "username"
                                    Email = reader.GetString(reader.GetOrdinal("email")), // Changed to "email"
                                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")), // Changed to "password_hash"
                                    ProfilePicture = !reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? reader.GetString(reader.GetOrdinal("profile_picture")) : null, // Added null check
                                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? null : reader.GetDateTime(reader.GetOrdinal("created_at")), // Added null check
                                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at")) // Added null check
                                };
                                Users.Add(user);
                            }
                            return await Task.FromResult(Users);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}