using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Pages
{
    public class ChatModel : PageModel
    {
        [BindProperty]
        public List<ChatMessage> Messages { get; private set; }

        [BindProperty]
        public int ProjectId { get; set; }
        [BindProperty]
        public Project Project { get; private set; }

        public async Task OnGet(int projectId)
        {
            this.ProjectId = projectId;
            try
            {
                Project = await ProjectController.GetProject(projectId);
                Messages = await ChatController.GetMessagesAsync(projectId);
                if (Messages == null)
                {
                    Messages = new List<ChatMessage>();
                }
                ViewData["ProjectId"] = ProjectId;
                ViewData["ProjectName"] = Project.ProjectName;
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
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
