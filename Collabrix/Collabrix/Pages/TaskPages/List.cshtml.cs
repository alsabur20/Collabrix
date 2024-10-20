using Microsoft.AspNetCore.Mvc.RazorPages;
using Collabrix.Models;
using Collabrix.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Collabrix.Pages.TaskPages
{
    public class ListModel : PageModel
    {
        private int _projectId;
        [BindProperty]
        public Project Project { get; set; }
        [BindProperty]
        public List<Tasks> Tasks { get;private set; }
        public async Task OnGet(int projectId)
        {
            try
            {
                _projectId = projectId;
                Project = await ProjectController.GetProject(_projectId);
                Tasks = await TaskController.GetTasks(_projectId);
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
        }
    }
}
