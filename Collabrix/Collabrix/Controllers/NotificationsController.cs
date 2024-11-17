using Collabrix.Models;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;

namespace Collabrix.Controllers
{
    public class NotificationsController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static async Task<List<Notification>> GetNotifications(int userId)
        {
            List<Notification> notifications = new List<Notification>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Notification WHERE RecipientId = @recepient";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@recepient", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Notification notif = new Notification
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Message = reader.GetString(reader.GetOrdinal("Message")),
                                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    NotificationTypeId = reader.GetInt32(reader.GetOrdinal("NotificationTypeId")),
                                    Read = reader.GetBoolean(reader.GetOrdinal("Read")),
                                    RecipientId = reader.GetInt32(reader.GetOrdinal("RecipientId")),
                                    SenderId = reader.IsDBNull(reader.GetOrdinal("SenderId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SenderId")),
                                    Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? "#" : reader.GetString(reader.GetOrdinal("Link"))
                                };
                                notifications.Add(notif);
                            }
                            return await Task.FromResult(notifications);
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

        public static async Task<string> GenerateNotificationsHtml(List<Notification> notifications)
        {
            StringBuilder notificationsHtml = new StringBuilder();

            notificationsHtml.AppendLine("<li>");
            notificationsHtml.AppendLine("    <div class='notif-scroll scrollbar-outer'>");
            notificationsHtml.AppendLine("        <div class='notif-center'>");

            foreach (var notif in notifications)
            {
                // Get the notification type and icon
                string type = await LookUpcontroller.GetValueById(notif.NotificationTypeId);
                string icon = string.Empty;
                string notifClass = string.Empty;
                string readClass = notif.Read ? "notif-read" : "notif-unread"; // Use different class for read/unread

                // Determine the icon and notification class based on the type
                if (type == "NewMessage")
                {
                    icon = "<i class='fa-solid fa-message'></i>";
                    notifClass = "notif-info";
                }
                else if (type == "ProjectUpdate")
                {
                    icon = "<i class='fa-solid fa-project-diagram'></i>";
                    notifClass = "notif-success";
                }
                else if (type == "TaskCompletion")
                {
                    icon = "<i class='fa-solid fa-check-circle'></i>";
                    notifClass = "notif-success";
                }
                else if (type == "DocumentUpload")
                {
                    icon = "<i class='fa-solid fa-file-upload'></i>";
                    notifClass = "notif-warning";
                }
                else if (type == "SystemAlert")
                {
                    icon = "<i class='fa-solid fa-bell'></i>";
                    notifClass = "notif-danger";
                }
                else if (type == "AssignmentDue")
                {
                    icon = "<i class='fa-solid fa-calendar'></i>";
                    notifClass = "notif-warning";
                }
                else if (type == "TaskAssignment")
                {
                    icon = "<i class='fa-solid fa-tasks'></i>";
                    notifClass = "notif-info";
                }
                else
                {
                    icon = "<i class='fa-solid fa-info-circle'></i>";
                    notifClass = "notif-primary";
                }

                // Calculate relative time
                TimeSpan timeDifference = DateTime.UtcNow - notif.CreatedDate;
                string relativeTime = timeDifference.TotalMinutes < 1 ? "Just now"
                : timeDifference.TotalMinutes < 60 ? $"{(int)timeDifference.TotalMinutes} minutes ago"
                : timeDifference.TotalHours < 24 ? $"{(int)timeDifference.TotalHours} hours ago"
                : $"{(int)timeDifference.TotalDays} days ago";

                // Append the notification HTML structure for each notification
                notificationsHtml.AppendLine("            <a href='" + HttpUtility.HtmlEncode(notif.Link) + "'>");
                notificationsHtml.AppendLine("                <div class='notif-icon " + notifClass + "'>");
                notificationsHtml.AppendLine("                    " + icon);
                notificationsHtml.AppendLine("                </div>");
                notificationsHtml.AppendLine("                <div class='notif-content " + readClass + "'>");
                notificationsHtml.AppendLine("                    <span class='block'>");
                notificationsHtml.AppendLine("                        " + notif.Message);
                notificationsHtml.AppendLine("                    </span>");
                notificationsHtml.AppendLine("                    <span class='time'>");
                notificationsHtml.AppendLine("                        " + relativeTime);
                notificationsHtml.AppendLine("                    </span>");
                notificationsHtml.AppendLine("                </div>");
                notificationsHtml.AppendLine("            </a>");
            }

            notificationsHtml.AppendLine("        </div>");
            notificationsHtml.AppendLine("    </div>");
            notificationsHtml.AppendLine("</li>");

            return notificationsHtml.ToString();
        }

    }
}
