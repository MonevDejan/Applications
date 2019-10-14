using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class TimeEntryService
    {
        public static TimeEntryClient client;
        
        public static async Task<ProjectInsight.Models.TimeAndExpense.TimeEntryInputSettingsAndLists> Get(
            Guid? selectedCompanyId = null,
            Guid? selectedProjectId = null,
            bool sortProjectListByProjectNumber = false,
            bool sortTaskDropDownListByTaskDate = false,
            bool sortTaskDropDownListByTaskNumber = false,
            bool includeCompletedTasksInList = false,
            bool includeUserTaskActualInputHours = false
            )
        {
            try
            {
                var result = await client.GetTimeEntryInputSettingsAndListsAsync(
                    Common.CurrentWorkspace.UserID,
                    selectedCompanyId,
                    selectedProjectId,
                    sortProjectListByProjectNumber,
                    sortTaskDropDownListByTaskDate,
                    sortTaskDropDownListByTaskNumber,
                    includeCompletedTasksInList, includeUserTaskActualInputHours
                    );

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public static ProjectInsight.Models.TimeAndExpense.TimeEntryInputSettingsAndLists GetSettings(
          Guid? selectedCompanyId = null,
          Guid? selectedProjectId = null,
          bool sortProjectListByProjectNumber = false,
          bool sortTaskDropDownListByTaskDate = false,
          bool sortTaskDropDownListByTaskNumber = false,
          bool includeCompletedTasksInList = false,
          bool includeUserTaskActualInputHours = false
          )
        {
            try
            {
                var result = client.GetTimeEntryInputSettingsAndLists(
                    Common.CurrentWorkspace.UserID,
                    selectedCompanyId,
                    selectedProjectId,
                    sortProjectListByProjectNumber,
                    sortTaskDropDownListByTaskDate,
                    sortTaskDropDownListByTaskNumber,
                    includeCompletedTasksInList, includeUserTaskActualInputHours
                    );

                return result;
            }
            catch (Exception ex)
            {
               
                return null;
            }


        }

        public static async Task<List<ProjectInsight.Models.TimeAndExpense.TimeEntry>> GetData(Guid userId, int? count = null)
        {
            try
            {
                var result = await client.GetByUserMostRecentlyUpdatedAsync(userId, count.Value, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,User,UserCreated,ExpenseCode,Project,Company,Department"));

                return result;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<bool> Save(ProjectInsight.Models.TimeAndExpense.TimeEntry item)
           {
            try
            {
                var result = await client.SaveAsync(item);

                return true;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
        }


    }

}
