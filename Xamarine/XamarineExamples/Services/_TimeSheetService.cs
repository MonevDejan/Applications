using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class TimeSheetService
    {
        public static TimeSheetClient client;
        public static async Task<List<TimeSheetInfo>> GetRecentTimeSheetInformation()
        {
            try
            {
                
                var res = await client.GetRecentTimeSheetInformationAsync();
                return res;

                //await Task.Factory.StartNew(() =>
                //{
                //    res = client.GetRecentTimeSheetInformation();
                //});
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static List<TimeSheet> GetTimesheetsForApproval()
        {
            try
            {
                return client.Search(userApproverGuidList: Common.CurrentWorkspace.UserID.ToString(), 
                    excludeApproved:true, excludePendingApproval:false, excludeRejected:true, 
                    modelProperties:new ProjectInsight.Models.Base.ModelProperties("default,User"));


            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public async static Task<TimeSheet> GetTimesheet(Guid id)
        {
            try
            {
                return await client.GetAsync(id);

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }
        public static ApiSaveResponse SubmitTimesheet(Guid id)
        {
            try
            {
                return client.SubmitTimesheet(id);

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static async Task<ApiSaveResponse> CreateTimeSheetAndSubmitForPeriod(DateTime StartDate, DateTime EndDate, Guid? approverId)
        {
            try
            {
                return await client.CreateTimeSheetAndSubmitForPeriodAsync(StartDate, EndDate, approverId);

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }

        public static async  Task<ApiSaveResponse> ApproveAsync(Guid TimeSheetd, string comments = "")
        {
            try
            {
                return await client.ApproveTimeSheetAsync(TimeSheetd, comments);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static async Task<ApiSaveResponse>  RejectAsync(TimeSheet item, string comments)
        {
            try
            {
                return await client.RejectTimeSheetAsync(item.Id.Value, comments);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<ApiDeleteResponse> Delete(Guid? TimeSheetId)
        {
            try
            {
                return await client.DeleteAsync(TimeSheetId);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }

}
