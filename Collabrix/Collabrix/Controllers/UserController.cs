using Collabrix.Models;
using System.Data.SqlClient;

namespace Collabrix.Controllers
{
    public class UserController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<User> GetUser(int userId)
        {
            User user = new User();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users WHERE UserId = @UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                user = new User
                                {
                                    UserId = userId,
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
            }
            return user;
        }

        public async static Task<List<User>> GetUsers()
        {
            List<User> users = new List<User>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users where isdeleted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
            }
            return users;
        }

        public async static Task<User> GetUser(string email, string passwordHash)
        {
            User? user = null;
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
            }
            return user;
        }

        public async static Task<int> AddUser(User user)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = @"
                INSERT INTO Users (FullName, Email, PasswordHash, CreatedAt, UpdatedAt, IsDeleted) 
                VALUES (@FullName, @Email, @PasswordHash, @CreatedAt, @UpdatedAt, @IsDeleted);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@IsDeleted", 0);

                        int userId = (int)await command.ExecuteScalarAsync();
                        return userId;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
            }
        }

        public async static Task<User> GetUser(string email)
        {
            User? user = null;
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
            }
            return user;
        }

        public async static Task<bool> UpdateUser(User user)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = @"
                        UPDATE Users 
                            SET FullName = @FullName, Email = @Email, PasswordHash = @PasswordHash, UpdatedAt = @UpdatedAt
                        WHERE UserId = @UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", user.UserId);
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        int affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
            }
        }
    }
}
