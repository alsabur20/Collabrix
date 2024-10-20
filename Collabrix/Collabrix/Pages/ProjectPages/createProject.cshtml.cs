using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;



namespace Collabrix.Pages
{
    public class createProjectModel : PageModel
    {
        private int _userId;
        // For Creation
        [BindProperty]
        public Project Project { get; set; }
        [BindProperty]
        public string? MembersList { get; set; }
        [BindProperty]
        public string? StagesList { get; set; }

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

        public async Task<IActionResult> OnPostCreateProject()
        {
            try
            {
                Project.CreatedBy = _userId;
                Project.CreatedAt = DateTime.Now;

                int createdProjectId = await ProjectController.CreateProject(Project);

                var selectedMembers = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(MembersList);
                List<UserProject> userProjects = new List<UserProject>();
                foreach (var member in selectedMembers)
                {
                    UserProject userProject = new UserProject
                    {
                        UserId = Convert.ToInt32(member["id"]),
                        ProjectId = createdProjectId,
                        CreatedAt = DateTime.Now,
                        IsDeleted = 1,
                        Role = Convert.ToInt32(member["role"])
                    };
                    userProjects.Add(userProject);
                }

                var selectedStages = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(StagesList);
                List<ProjectTaskStage> projectStages = new List<ProjectTaskStage>();
                foreach(var stage in selectedStages)
                {
                    ProjectTaskStage projectTaskStage = new ProjectTaskStage
                    {
                        ProjectId = createdProjectId,
                        StageName = stage["name"],
                        StageDescription = stage["description"],
                        CreatedBy = _userId,
                        IsDeleted = 1
                    };
                    projectStages.Add(projectTaskStage);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return Page();
        }
    }
}
