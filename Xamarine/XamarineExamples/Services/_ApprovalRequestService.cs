using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.ApprovalRequests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ApprovalRequestService
    {
        public static ApprovalRequestClient client;
        public static ApprovalRequestItemClient clientItem;

        public static async Task<ProjectInsight.Models.ApprovalRequests.ApprovalRequest> GetApprovalRequest(Guid guid)
        {
            return await client.GetAsync(guid, 
                modelProperties: 
                new ProjectInsight.Models.Base.ModelProperties("default,ItemNumberFullAndNameDisplayPreference,ApprovalRequestStateType,RequiresApprovalFromCurrentUser,ApprovalRequestStateTypeDescription,Description,ApprovedDateTimeUTC,DeadlineDateTimeUTC,CreatedDateTimeUTC,ApprovalRequestApprovals,ApprovalRequestItems,Requestor;Requestor:Name;ApprovalRequestApproval:Approver;User:Name,AvatarHtml,PhotoUrl;ApprovalRequestItem:default,Item_Id,ItemType,StartDate,EndDate,ProjectStatus;"));

                
        }
        public static async Task<ApiSaveResponse> Save(ProjectInsight.Models.ApprovalRequests.ApprovalRequest model)
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

        public static async Task<ApiSaveResponse> Update(ProjectInsight.Models.ApprovalRequests.ApprovalRequest model)
        {
            return await client.SaveAsync(model);
        }
    }

}
