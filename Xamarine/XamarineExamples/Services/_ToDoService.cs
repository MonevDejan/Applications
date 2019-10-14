using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.ToDos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ToDoService
    {
        public static ToDoClient client;
        public static async Task<ProjectInsight.Models.ToDos.ToDo> GetItem(Guid itemId)
        {
            try
            {
                return await client.GetAsync(itemId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,UserCreated,UserAssignedBy,UserAssignedTo,UserAssignedTo_Id,WorkPercentCompleteType,StartDateTimeUserLocal,EndDateTimeUserLocal,ProjectAffiliation,ProjectAffiliation_Id,StartDateTimeUTC,EndDateTimeUTC;Project:ItemNumberFullAndNameDisplayPreference"));
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<ApiSaveResponse> SaveTodo(ProjectInsight.Models.ToDos.ToDo model)
        {
            try
            {
                var resp = await client.SaveAsync(model);
                return resp;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;

            }
        }
    }

}
