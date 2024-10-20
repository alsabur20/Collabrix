using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;


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
        [BindProperty]
        public Dictionary<string, int> ProjectStages { get; set; }


        public async void OnGet()
        {
            try
            {
                Statuses = await LookUpcontroller.getStatuses();
                Statuses.Insert(0, "Select Project Status");
                ProjectTypes = await LookUpcontroller.getProjectTypes();
                ProjectTypes.Insert(0, "Select Project Type");
            }
            catch
            {

            }

        }

        public async void OnPostCreateProjectAsync()
        {
            try
            { 
                Project.ProjectType = await LookUpcontroller.getIdByValue(ProjectType);
                Project.Status = await LookUpcontroller.getIdByValue(Status);
                Project.CreatedBy = 1;
                int projectId =  await ProjectController.CreateProject(Project);
                if(ProjectStages != null) {
                    foreach (var item in ProjectStages)
                    {
                        ProjectTaskStage projectTaskStage = new ProjectTaskStage
                        {
                            ProjectId = projectId,
                            StageName = item.Key,
                            StageDescription = item.Value.ToString(),
                            CreatedBy = 1
                        };
                        await ProjectTaskStageController.CreateProjectTaskStage(projectTaskStage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }

        }

        public async void OnPostCreateStageAsync()
        {
            try
            {
                ProjectTaskStage projectTaskStage = new ProjectTaskStage
                {
                    ProjectId = 1,
                    StageName = stageName,
                    StageDescription = stageDescription,
                    CreatedBy = 1
                };
                await ProjectTaskStageController.CreateProjectTaskStage(projectTaskStage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }
        }
    }
}   
