using Microsoft.AspNetCore.Mvc;
using Collabrix.Models;
using Collabrix.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.HttpSys;
using Collabrix.Helper;

namespace Collabrix.Pages
{
    public class allProjectsModel : PageModel
    {
        [BindProperty]
        public  List<Tuple<Project, string, string>> Projects { get; set; }

        public async void OnGet()
        {
            try
            {
                this.Projects = new List<Tuple<Project, string, string>>();
                List<Project> projects = ProjectController.GetProjects(1).Result;
                foreach (Project project in projects)
                {
                    string leader = await UserProjectController.getLeader(project.ProjectId);
                    string initials = helper.GetInitials(leader);
                    this.Projects.Add(new Tuple<Project, string, string>(project, leader, initials));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
        }
    }
}
