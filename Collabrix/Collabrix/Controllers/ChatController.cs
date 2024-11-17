using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Controllers
{
    public class ChatController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Retrieve all chat messages
        public static async Task<List<ChatMessage>> GetMessagesAsync(int projectId)
        {
            var messages = new List<ChatMessage>();
            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("Default")))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM ChatMessages WHERE ProjectId = @ProjectId ORDER BY SentAt";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messages.Add(new ChatMessage
                            {
                                MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                                ProjectId = projectId,
                                MessageText = reader.GetString(reader.GetOrdinal("MessageText")),
                                SentBy = reader.GetInt32(reader.GetOrdinal("SentBy")),
                                SentAt = reader.GetDateTime(reader.GetOrdinal("SentAt"))
                            });
                        }
                        return await Task.FromResult(messages);
                    }
                }
            }
        }

        // Add a new message to the database
        public static async Task AddMessageAsync(ChatMessage message)
        {
            string connectionString = Configuration.GetConnectionString("Default");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO ChatMessages (ProjectId, MessageText, SentBy, SentAt) VALUES (@ProjectId, @MessageText, @SentBy, @SentAt)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", message.ProjectId);
                    command.Parameters.AddWithValue("@MessageText", message.MessageText);
                    command.Parameters.AddWithValue("@SentBy", message.SentBy);
                    command.Parameters.AddWithValue("@SentAt", message.SentAt);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
