﻿using Collabrix.Models;
using System.Data.SqlClient;

namespace Collabrix.Controllers
{
    public class ProjectTaskStageController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<List<ProjectTaskStage>> GetProjectTaskStages(int projectId)
        {
            List<ProjectTaskStage> projectTaskStages = new List<ProjectTaskStage>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM ProjectTaskStage WHERE ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                ProjectTaskStage projectTaskStage = new ProjectTaskStage
                                {
                                    StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    StageName = reader.GetString(reader.GetOrdinal("StageName")),
                                    StageDescription = reader.GetString(reader.GetOrdinal("StageDescription")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    IsDeleted = reader.GetInt32(reader.GetOrdinal("IsDeleted"))
                                };
                                projectTaskStages.Add(projectTaskStage);
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
            return projectTaskStages; // Moved return statement outside the try block
        }

        public async static Task<ProjectTaskStage> GetProjectTaskStage(int stageId)
        {
            ProjectTaskStage projectTaskStage = new ProjectTaskStage();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM ProjectTaskStage WHERE StageId = @StageId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StageId", stageId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                projectTaskStage = new ProjectTaskStage
                                {
                                    StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    StageName = reader.GetString(reader.GetOrdinal("StageName")),
                                    StageDescription = reader.GetString(reader.GetOrdinal("StageDescription")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
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
            return projectTaskStage; // Moved return statement outside the try block
        }

        public async static Task<int> CreateProjectTaskStage(ProjectTaskStage projectTaskStage)
        {
            int stageId = 0;
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO ProjectTaskStage (ProjectId, StageName, StageDescription, CreatedBy, IsDeleted) VALUES (@ProjectId, @StageName, @StageDescription, @CreatedBy,(SELECT lookupId FROM Lookup Where value = 'Active' ))";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectTaskStage.ProjectId);
                        command.Parameters.AddWithValue("@StageName", projectTaskStage.StageName);
                        command.Parameters.AddWithValue("@StageDescription", projectTaskStage.StageDescription);
                        command.Parameters.AddWithValue("@CreatedBy", projectTaskStage.CreatedBy); // Use ExecuteScalarAsync for better async support
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
            return stageId; // Moved return statement outside the try block
        }
    }
}
