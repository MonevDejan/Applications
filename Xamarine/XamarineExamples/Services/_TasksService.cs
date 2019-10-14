using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class TasksService
    {
        public static TaskClient client;
        public static async Task<ProjectInsight.Models.Tasks.Task> GetTask(Guid guid)
        {
            try
            {
               return await client.GetAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,ItemNumberFullAndNameDisplayPreference,DurationString,Description,UserCreated,UserAssignedBy,UserAssignedTo,TaskOwner_Id,WorkPercentCompleteType,StartDateTimeUTC,EndDateTimeUTC,WorkHours,DurationSeconds,Project;Project:ItemNumberFullAndNameDisplayPreference,StartDateTimeUTC,EndDateTimeUTC"));

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<ApiSaveResponse> Save(ProjectInsight.Models.Tasks.Task model)
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

        public static async Task<List<ProjectInsight.Models.Tasks.Task>> GetByProject(Guid guid)
        {
            try
            {
                //return await client.GetByProjectAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,UserCreated,UserAssignedBy,UserAssignedTo,WorkPercentCompleteType,StartDateTimeUserLocal,EndDateTimeUserLocal,WorkHours,DurationSeconds,Project"));
                return await client.GetByProjectAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,TaskOwner_Id,TaskOwner,WorkPercentCompleteType,StartDateTimeUserLocal,EndDateTimeUserLocal,TaskScheduleCurrentState;User:FirstName,LastName"));
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<List<ProjectInsight.Models.Tasks.Task>> GetAllForUser()
        {
            //TEST 
            List<ProjectInsight.Models.Tasks.Task> tasks = new List<ProjectInsight.Models.Tasks.Task>();
            var task1= await client.GetAsync(new Guid("5408a5dcecee426a8ce655736f8127c0"));
            var task2 = await client.GetAsync(new Guid("8680da19ee5b4247b9bbcede65136c44"));
            tasks.Add(task1);
            tasks.Add(task2);

            return tasks;
        }

    }

}
