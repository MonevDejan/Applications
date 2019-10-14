using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.ApprovalRequests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ApprovalRequestApprovalService
    {
        public static ApprovalRequestApprovalClient client;
       
        public static async Task<ApiSaveResponse> Save(ProjectInsight.Models.ApprovalRequests.ApprovalRequestApproval model)
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
