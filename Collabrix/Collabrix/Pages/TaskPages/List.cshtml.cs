using Microsoft.AspNetCore.Mvc.RazorPages;
using Collabrix.Models;
using Collabrix.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Collabrix.Pages.TaskPages
{
    public class ListModel : PageModel
    {
        [BindProperty]
        public int ProjectId { get; set; }
        [BindProperty]
        public Project? Project { get; private set; }
        [BindProperty]
        public List<Tasks>? Tasks { get; private set; }
        [BindProperty]
        public List<ProjectTaskStage>? Stages { get; private set; }
        [BindProperty]
        public List<UserProject>? Members { get; private set; }
        public async Task OnGet(int projectId)
        {
            try
            {
                ProjectId = projectId;
                Project = await ProjectController.GetProject(ProjectId);
                Tasks = await TaskController.GetTasks(ProjectId);
                Stages = await ProjectTaskStageController.GetProjectTaskStages(ProjectId);
                Members = await UserProjectController.GetMembers(ProjectId);
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
        }
    }
}
