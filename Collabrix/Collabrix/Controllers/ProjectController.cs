using Collabrix.Models;
using System.Data;
using System.Data.SqlClient;

namespace Collabrix.Controllers
{
    public class ProjectController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<Project> GetProject(int projectId)
        {
            Project? project = null;
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Projects WHERE ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                project = new Project
                                {
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                    ProjectType = reader.GetInt32(reader.GetOrdinal("ProjectTypeLookupId")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetInt32(reader.GetOrdinal("IsDeleted"))
                                };
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
            return project; // Moved return statement outside the try block
        }

        public async static Task<List<Project>> GetProjects(int userId)
        {
            List<Project> projects = new List<Project>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = $"SELECT * FROM Projects WHERE ProjectId IN (SELECT ProjectId From UserProject WHERE UserId = {userId})";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                Project project = new Project
                                {
                                   ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")), 
                                   ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")), 
                                   Description = reader.GetString(reader.GetOrdinal("Description")), 
                                   StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")), 
                                   EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")), 
                                   ProjectType = reader.GetInt32(reader.GetOrdinal("ProjectTypeLookupId")), 
                                   CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                   CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")), 
                                   UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")), 
                                   IsDeleted = reader.GetInt32(reader.GetOrdinal("IsDeleted")) 

                                };
                                projects.Add(project);
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
            return projects; // Moved return statement outside the try block
        }

        public async static Task<int> CreateProject(Project project)
        {
            string connectionString = Configuration.GetConnectionString("Default");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("stpCreateProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                        command.Parameters.AddWithValue("@Description", project.Description);
                        command.Parameters.AddWithValue("@StartDate", project.StartDate);
                        command.Parameters.AddWithValue("@EndDate", project.EndDate);
                        command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
                        command.Parameters.AddWithValue("@CreatedBy", project.CreatedBy);

                        SqlParameter projectIdParam = new SqlParameter("@ProjectId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(projectIdParam);

                        await command.ExecuteNonQueryAsync();

                        int projectId = (int)command.Parameters["@ProjectId"].Value;

                        return projectId; 
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}
