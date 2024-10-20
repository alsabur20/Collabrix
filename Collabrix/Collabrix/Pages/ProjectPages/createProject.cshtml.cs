using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Pages
{
    public class createProjectModel : PageModel
    {
        [BindProperty]
        public Project Project { get; set; }
        [BindProperty]
        public string Status { get; set; }
        [BindProperty]
        public string ProjectType { get; set; }
        [BindProperty]
        public List<string> Statuses {  get; set; } 
        [BindProperty]
        public List<string> ProjectTypes { get; set; }
        [BindProperty]
        public string stageName { get; set; }
        [BindProperty]
        public string stageDescription { get; set; }

        public async void OnGet()
        {
            try
            {
                Statuses = await LookUpcontroller.getStatuses();
                Statuses.Insert(0, "Select Project Status");
                ProjectTypes = await LookUpcontroller.getProjectTypes();
                ProjectTypes.Insert(0, "Select Project Type");
            }
            catch {

            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public async void OnPostCreateProjectAsync()
        {
            try
            {
                Project.ProjectType = await LookUpcontroller.getIdByValue(ProjectType);
                Project.Status = await LookUpcontroller.getIdByValue(Status);
                Project.CreatedBy = 1;
                await ProjectController.CreateProject(Project);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }

        }
    }
}   
