using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace Collabrix.Pages.Notifications
{
    public class NotificationsModel : PageModel
    {
        public List<Notification> Notifications { get; set; } = new();

        [BindProperty]
        public int NotificationId { get; set; }

        public async void OnGet()
        {
            var userId = User.FindFirst("uId")?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int uId))
            {
                TempData["ErrorOnServer"] = "Invalid user ID.";
                return;
            }

            User us = await UserController.GetUser(uId);
            if (us == null)
            {
                TempData["ErrorOnServer"] = "User not found.";
            }

            Notifications = await NotificationsController.GetNotifications(uId);
        }

        public IActionResult OnPostMarkRead()
        {
            if (NotificationId == 0)
            {
                TempData["Error"] = "Invalid notification ID.";
                return RedirectToPage();
            }

            if (NotificationsController.MarkNotificationAsRead(NotificationId).Result)
            {
                TempData["Success"] = $"Notification {NotificationId} marked as read.";
            }
            else
            {
                TempData["Error"] = $"Failed to mark notification {NotificationId} as read.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete()
        {
            if (NotificationId == 0)
            {
                TempData["Error"] = "Invalid notification ID.";
                return RedirectToPage();
            }
            if (NotificationsController.DeleteNotification(NotificationId).Result)
            {
                TempData["Success"] = $"Notification {NotificationId} deleted.";
            }
            else
            {
                TempData["Error"] = $"Failed to delete notification {NotificationId}.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostMarkAllRead()
        {
            var userId = User.FindFirst("uId")?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int uId))
            {
                TempData["ErrorOnServer"] = "Invalid user ID.";
                return RedirectToPage();
            }

            if (NotificationsController.MarkAllNotificationsAsRead(uId).Result)
            {
                TempData["Success"] = "All notifications marked as read.";
            }
            else
            {
                TempData["Error"] = "Failed to mark all notifications as read.";
            }
            return RedirectToPage();
        }
    }
}
