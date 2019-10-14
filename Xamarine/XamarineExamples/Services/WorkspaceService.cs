using ProjectInsight.Models.Users;
using ProjectInsight.Models.Workspace;
using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsight.WebApi.Client.Users;
using ProjectInsight.WebApi.Client.Workspace;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class WorkspaceService
    {
        public static WorkspaceClient client;
        public WorkspaceService()
        {
            
        }
       

        public static WorkspaceCapability GetWorkspaceCapability()
        {
            try
            {
                WorkspaceCapability workspaceCapability = client.GetWorkspaceCapability();
                return workspaceCapability;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WorkspaceCapability> GetWorkspaceCapabilityAsync()
        {
            try
            {
                WorkspaceCapability workspaceCapability = await client.GetWorkspaceCapabilityAsync();
                return workspaceCapability;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
