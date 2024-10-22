using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Collabrix.Pages.ProjectPages
{
    public class ProjectSettingModel : PageModel
    {
        private int _userId;
        [BindProperty]
        public Project Project { get; set; }

        private int projectId;
        public List<Lookup>? Statuses { get; private set; }
        public List<Lookup>? ProjectTypes { get; private set; }
        public List<Lookup>? Roles { get; private set; }
        public List<User>? Users { get; private set; }
        public List<UserProject>? ProjectUsers { get; private set; }

        public async Task OnGet(int projectId)
        {
            try
            {
                this.projectId = projectId;
                Project = await ProjectController.GetProject(this.projectId);
                ProjectUsers = await UserProjectController.GetProjectUsers(this.projectId);
                _userId = 1;
                Statuses = await LookUpcontroller.GetLookupsByategory("Status");
                ProjectTypes = await LookUpcontroller.GetLookupsByategory("ProjectType");
                Roles = await LookUpcontroller.GetLookupsByategory("Role");
                Users = await UserController.GetUsers();
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
        }
    }
}
