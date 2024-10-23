using Collabrix.Models;
using System.Data.SqlClient;

namespace Collabrix.Controllers
{
    public class TaskController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task ChangeTaskStage(int taskId, int newStageId)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "UPDATE Tasks SET ProjectTaskStageId = @NewStageId WHERE TaskId = @TaskId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewStageId", newStageId);
                        command.Parameters.AddWithValue("@TaskId", taskId);
                        await command.ExecuteNonQueryAsync(); // Use ExecuteNonQueryAsync for better async support
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

        public async static Task<Tasks> GetTask(int taskId)
        {
            Tasks task = new Tasks();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Tasks WHERE TaskId = @TaskId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskId", taskId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                task.TaskId = reader.GetInt32(reader.GetOrdinal("TaskId"));
                                task.TaskName = reader.GetString(reader.GetOrdinal("TaskName"));
                                task.Description = reader.GetString(reader.GetOrdinal("Description"));
                                task.DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate"));
                                task.ProjectTaskStageId = reader.GetInt32(reader.GetOrdinal("ProjectTaskStageId"));
                                task.AssignedTo = reader.GetInt32(reader.GetOrdinal("AssignedTo"));
                                task.ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId"));
                                task.CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"));
                                task.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                task.UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
                                task.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            }
                            return await Task.FromResult(task);
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

        public async static Task<List<Tasks>> GetTasks(int projectId)
        {
            List<Tasks> tasks = new List<Tasks>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Tasks WHERE ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                Tasks task = new Tasks
                                {
                                    TaskId = reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    TaskName = reader.GetString(reader.GetOrdinal("TaskName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                    ProjectTaskStageId = reader.GetInt32(reader.GetOrdinal("ProjectTaskStageId")),
                                    AssignedTo = reader.GetInt32(reader.GetOrdinal("AssignedTo")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                                tasks.Add(task);
                            }
                            return await Task.FromResult(tasks);
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
