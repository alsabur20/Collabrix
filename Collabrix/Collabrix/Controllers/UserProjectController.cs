using Collabrix.Models;
using System.Data.SqlClient;

namespace Collabrix.Controllers

{
    public class UserProjectController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<List<UserProject>> GetUserProjects(int userId)
        {
            List<UserProject> userProjects = new List<UserProject>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = $"SELECT * FROM UserProject WHERE UserId = {userId}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                UserProject userProject = new UserProject
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    IsDeleted = reader.GetInt32(reader.GetOrdinal("IsDeleted"))
                                };
                                userProjects.Add(userProject);
                            }
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
            return userProjects;
        }

        public async static Task<string> getLeader(int projectId)
        { 
            string leader = "";
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = $"SELECT u.FullName From UserProject UP\r\nJOIN Users u\r\nOn UP.UserId = u.UserId\r\nWHERE ProjectId = {projectId} AND Role = (Select lookupid from lookup where value = 'Team Leader')";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                leader = reader.GetString(reader.GetOrdinal("FullName"));
                            }
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
            return leader;
        }
    }
}
