using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Pages
{
    public class messageModel : PageModel
    {
        [BindProperty]
        public List<ChatMessage> Messages { get; private set; }

        [BindProperty]
        public int ProjectId { get; set; }

        public async Task<IActionResult> OnGet(int projectId)
        {
            this.ProjectId = projectId;
            try
            {
                Messages = await ChatController.GetMessagesAsync(projectId);
                if (Messages == null)
                {
                    Messages = new List<ChatMessage>();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
            return Page();
        }

        public async Task<string> GetUserName(int userId)
        {
            User user = await UserController.GetUser(userId);
            if (user != null)
                return user.FullName;
            else
                return "Null";
        }
    }
}
