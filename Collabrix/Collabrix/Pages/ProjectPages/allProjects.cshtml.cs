using Collabrix.Controllers;
using Collabrix.Helper;
using Collabrix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Pages
{
    public class allProjectsModel : PageModel
    {
        public List<Tuple<Project, string, string>> Projects { get; set; }

        public async Task OnGet()
        {
            try
            {
                int userId = GetUId();
                if (userId == -1)
                {
                    TempData["ErrorOnServer"] = "User not found";
                    return;
                }
                Projects = new List<Tuple<Project, string, string>>();
                List<Project> projects = await ProjectController.GetProjects(userId);
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
        private int GetUId()
        {
            var uIdClaim = User.FindFirst("uId");
            if (uIdClaim == null)
            {
                return -1;
            }
            return int.Parse(uIdClaim.Value);
        }
    }
}
