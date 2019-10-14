using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.Projects;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ProjectsService
    {
        public static ProjectClient client;
        public static async Task<ProjectInsight.Models.Projects.Project> GetProject(Guid guid)
        {
            try
            {
                return await client.GetAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,TaskCount,Description,UserCreated,UserAssignedBy,UserAssignedTo,WorkPercentCompleteType,StartDateTimeUserLocal,EndDateTimeUserLocal,DurationSeconds,WorkHours,ProjectStatus,ItemNumberFullAndNameDisplayPreference,ProjectResources"));
                //return await client.GetAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("all"));

            }
            catch (Exception ex)
            {
              //  AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<List<ProjectInsight.Models.Projects.Project>> GetAllProjects()
        {
            try
            {
                return await client.SearchAsync(userId: Common.CurrentWorkspace.UserID, orderBy:"Name", isActive:true);
                //return await client.GetAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("all"));

            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<List<ProjectInsight.Models.Projects.Project>> GetAllProjectsActiveAndPlanningUserCanView()
        {
            try
            {
                return await client.ActiveAndPlanningUserCanViewAsync(new ProjectInsight.Models.Base.ModelProperties("default,ItemNumberFullAndNameDisplayPreference,UrlIcon,UrlThumbnail"));
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }
        public static async Task<ApiSaveResponse> Save(ProjectInsight.Models.Projects.Project model)
        {
            try
            {
                return await client.SaveAsync(model);
            }
            catch (Exception ex)
            {
               //AuthenticationService.Logout();
                return null;
            }
        }

    }

}
