using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.SignalR;

namespace Collabrix.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, int> UserProjectMap = new Dictionary<string, int>();

        public async Task JoinProjectGroup(int projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Project_{projectId}");
            UserProjectMap[Context.ConnectionId] = projectId;

            string userName = Context.User?.Identity?.Name ?? "Guest";
            await Clients.Group($"Project_{projectId}").SendAsync("UserJoined", userName, projectId);

            Console.WriteLine($"User {userName} joined group: Project_{projectId}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (UserProjectMap.TryGetValue(Context.ConnectionId, out int projectId))
            {
                UserProjectMap.Remove(Context.ConnectionId);

                string userName = Context.User?.Identity?.Name ?? "Guest";
                await Clients.Group($"Project_{projectId}").SendAsync("UserLeft", userName, projectId);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Project_{projectId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // The SendMessage method
        public async Task SendMessage(int projectId, string message)
        {
            string user = Context.User?.Identity?.Name ?? "Guest";
            string userIdClaim = Context.User?.FindFirst("uId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                await Clients.Caller.SendAsync("ErrorMessage", "User is not authorized or invalid.");
                return;
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
                await Clients.Group($"Project_{projectId}").SendAsync("ReceiveMessage", user, message, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                await Clients.Caller.SendAsync("ErrorMessage", "Failed to send message. Please try again.");
            }
        }

    }
}
