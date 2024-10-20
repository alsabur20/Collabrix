using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Collabrix.Pages
{
    public class createProjectModel : PageModel
    {
        private int _userId;
        // For Creation
        [BindProperty]
        public Project Project { get; set; }

        // For Displaying
        public List<Lookup>? Statuses { get; private set; }
        public List<Lookup>? ProjectTypes { get; private set; }
        public List<Lookup>? Roles { get; private set; }
        public List<User>? Users { get; private set; }

        public string? stageName { get; set; }
        [BindProperty]
        public string? stageDescription { get; set; }
        [BindProperty]
        public Dictionary<string, int>? ProjectStages { get; set; }


        public async void OnGet(int userId)
        {
            try
            {
                _userId = 1;
                Statuses = await LookUpcontroller.GetLookupsByategory("ProjectStatus");
                ProjectTypes = await LookUpcontroller.GetLookupsByategory("ProjectType");
                Roles = await LookUpcontroller.GetLookupsByategory("Role");
                Users = await UserController.GetUsers();
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }

        }

        public async Task<IActionResult> OnPostCreateProjectAsync()
        {
            try
            {
                Project.CreatedBy = _userId;
                Project.CreatedAt = DateTime.Now;
                //int projectId = await ProjectController.CreateProject(Project);
                //if (ProjectStages != null)
                //{
                //    foreach (var item in ProjectStages)
                //    {
                //        ProjectTaskStage projectTaskStage = new ProjectTaskStage
                //        {
                //            ProjectId = projectId,
                //            StageName = item.Key,
                //            StageDescription = item.Value.ToString(),
                //            CreatedBy = 1
                //        };
                //        await ProjectTaskStageController.CreateProjectTaskStage(projectTaskStage);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }
            return RedirectToPage("/ProjectPages/createProject");
        }

        public async void OnPostCreateStageAsync()
        {
            try
            {
                //ProjectTaskStage projectTaskStage = new ProjectTaskStage
                //{
                //    ProjectId = 1,
                //    StageName = stageName,
                //    StageDescription = stageDescription,
                //    CreatedBy = 1
                //};
                //await ProjectTaskStageController.CreateProjectTaskStage(projectTaskStage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }
        }
    }
}
