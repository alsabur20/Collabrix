using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.SignalR;

namespace Collabrix.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, int> UserProjectMap = new Dictionary<string, int>();

        // Method to add the user to a project group
        public async Task JoinProjectGroup(int projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Project_{projectId}");
            UserProjectMap[Context.ConnectionId] = projectId;
            Console.WriteLine($"User joined group: Project_{projectId}");
        }

        // Override OnDisconnectedAsync to clean up when the user disconnects
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (UserProjectMap.TryGetValue(Context.ConnectionId, out int projectId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Project_{projectId}");
                UserProjectMap.Remove(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // The SendMessage method
        public async Task SendMessage(int projectId, string message)
        {
            string user = Context.User?.Identity?.Name ?? "Guest";
            int userId = int.Parse(Context.User?.FindFirst("uId")?.Value ?? "-1");
            if (userId == -1)
            {
                throw new Exception("User not found");
            }

            try
            {
                var chatMessage = new ChatMessage
                {
                    SentBy = userId,
                    MessageText = message,
                    ProjectId = projectId,
                    SentAt = DateTime.Now
                };

                await ChatController.AddMessageAsync(chatMessage);
                await Clients.Group($"Project_{projectId}").SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw;
            }
        }
    }
}
