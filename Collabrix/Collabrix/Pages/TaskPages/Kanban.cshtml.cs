using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Pages.TaskPages
{
    public class KanbanModel : PageModel
    {
        private int _projectId;
        [BindProperty]
        public Project Project { get; set; } = new Project();
        [BindProperty]
        public List<ProjectTaskStage> Stages { get; private set; } = new List<ProjectTaskStage>();
        [BindProperty]
        public List<Tasks> Tasks { get; private set; } = new List<Tasks>();
        [BindProperty]
        public List<User> Users { get; private set; } = new List<User>();

        public async Task<IActionResult> OnGet(int projectId)
        {
            try
            {
                _projectId = projectId;

                // Fetch project data
                Project = await ProjectController.GetProject(_projectId);
                if (Project == null)
                {
                    TempData["ErrorOnServer"] = "No project found with the specified ID.";
                    Project = new Project(); // Initialize an empty project to prevent null references
                }

                // Fetch project task stages
                Stages = await ProjectTaskStageController.GetProjectTaskStages(_projectId);
                if (Stages == null || !Stages.Any())
                {
                    TempData["ErrorOnServer"] += "\nNo stages found for the project.";
                    Stages = new List<ProjectTaskStage>(); // Ensure Stages is initialized
                }

                // Fetch users
                Users = await UserController.GetUsers();
                if (Users == null || !Users.Any())
                {
                    TempData["ErrorOnServer"] += "\nNo users found.";
                    Users = new List<User>(); // Ensure Users is initialized
                }

                // Fetch tasks
                Tasks = await TaskController.GetTasks(_projectId);
                if (Tasks == null)
                {
                    TempData["ErrorOnServer"] += "\nNo tasks found for the project.";
                    Tasks = new List<Tasks>(); // Ensure Tasks is initialized
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] += "\n" + ex.Message;
            }

            // Render the page
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateTaskStage(int taskId, int newStageId)
        {
            try
            {
                await TaskController.ChangeTaskStage(taskId, newStageId);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
