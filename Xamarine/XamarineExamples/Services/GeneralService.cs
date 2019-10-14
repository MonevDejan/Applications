using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.ReferenceData;
using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class WorkPercentCompleteTypeService
    {
        public static WorkPercentCompleteTypeClient client;
        public static async Task<List<ProjectInsight.Models.ReferenceData.WorkPercentCompleteType>> GetAllTypes()
        {
            try
            {
               return await client.ListActiveAsync(modelProperties: new ProjectInsight.Models.Base.ModelProperties("default"));

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }
    }

    public class ProjectStatusService
    {
        public static ProjectStatusClient client;
        public static async Task<List<ProjectInsight.Models.ReferenceData.ProjectStatus>> GetAllStatuses()
        {
            try
            {
                return await client.ListActiveAsync(modelProperties: new ProjectInsight.Models.Base.ModelProperties("default"));

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;
            }
        }
    }

    public class ProjectTypeService
    {
        public static ProjectTypeClient client;
        public static async Task<List<ProjectInsight.Models.ReferenceData.ProjectType>> GetAllTypes()
        {
            try
            {
                return await client.ListActiveAsync(modelProperties: new ProjectInsight.Models.Base.ModelProperties("default"));

            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;
            }
        }
    }
    public class IssueStatusTypeService
    {
        public static IssueStatusTypeClient client;
    }

    public class IssueTypeService
    {
        public static IssueTypeClient client;
    }

    public class IssueSeverityTypeService
    {
        public static IssueSeverityTypeClient client;
    }
    public class IssuePriorityTypeService
    {
        public static IssuePriorityTypeClient client;
    }
}

