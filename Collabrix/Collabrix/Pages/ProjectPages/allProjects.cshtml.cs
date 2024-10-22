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
        public  List<Tuple<Project, string, string>> Projects { get; set; }

        public async Task OnGet()
        {
            try
            {
                Projects = new List<Tuple<Project, string, string>>();
                List<Project> projects = await ProjectController.GetProjects(1);
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
