using ProjectInsight.Models.Users;
using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsight.WebApi.Client.Users;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class UsersService
    {
        public static UserClient client;
       

        public UsersService()
        {
            
        }
        public static MyWork GetMyWork(Guid userId)
        {
         

            try
            {
               MyWork myWorkItems = client.GetMyWork(userId, 
                    new ProjectInsight.Models.Base.ModelProperties(
                    "Task:Id,Name,StartDateTimeUserLocal,EndDateTimeUserLocal,ItemNumberFullAndNameDisplayPreference,Project_Id,Project;" +
                    "Issue:Id,Name,StartDateTimeUserLocal,EndDateTimeUserLocal,IssuePriority,IssueStatusType,ProjectResolution_Id,ProjectAffiliation;" +
                    "ToDo:Id,Name,StartDateTimeUserLocal,EndDateTimeUserLocal,ProjectAffiliation_Id,ProjectAffiliation;" +
                    "ApprovalRequest:Id,Name,DeadlineDateTimeUTC,ApprovalRequestStateType;" +
                    "Project:Id,Name,TaskCount,ItemNumberFullAndNameDisplayPreference,StartDateTimeUserLocal,EndDateTimeUserLocal,ProjectStatus,ProjectState,PrimaryProjectManager;" +
                    "User:FirstName,LastName,Name;"));

                return myWorkItems;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;

            }

        }


        public static async Task<MyWork> GetMyWorkAsync(Guid userId)
        {


            try
            {
                MyWork myWorkItems = await client.GetMyWorkAsync(userId,
                     new ProjectInsight.Models.Base.ModelProperties(
                     "Task:Id,Name,StartDateTimeUTC,EndDateTimeUTC,ItemNumberFullAndNameDisplayPreference,Project_Id,Project;" +
                     "Issue:Id,Name,StartDateTimeUTC,EndDateTimeUTC,IssuePriority,IssueStatusType,ProjectResolution_Id,ProjectAffiliation_Id,ProjectAffiliation;" +
                     "ToDo:Id,Name,StartDateTimeUTC,EndDateTimeUTC,ProjectAffiliation_Id,ProjectAffiliation;" +
                     "ApprovalRequest:Id,Name,DeadlineDateTimeUTC,ApprovalRequestApprovals,ApprovalRequestStateType;ApprovalRequestApproval:Approver;" +
                     "Project:Id,Name,TaskCount,ItemNumberFullAndNameDisplayPreference,StartDateTimeUTC,EndDateTimeUTC,ProjectStatus,ProjectState,PrimaryProjectManager,Companies;" +
                     "TimeSheet:default,Status;" +
                     "User:FirstName,LastName,Name;"));

                //if (myWorkItems != null && myWorkItems.TimeSheets != null)
                //{
                //    foreach (var ts in myWorkItems.TimeSheets)
                //    {
                //        myWorkItems.ItemsInOrder.Add(ts);
                //    }
                //}
                return myWorkItems;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;

            }

        }
        public static async Task<MyWork> GetMyProjectsAsync(Guid userId)
        {


            try
            {
                MyWork myWorkItems = await client.GetMyWorkAsync(userId,
                     new ProjectInsight.Models.Base.ModelProperties(
                     "Project:Id,Name,TaskCount,ItemNumberFullAndNameDisplayPreference,StartDateTimeUTC,EndDateTimeUTC,ProjectStatus,ProjectState,PrimaryProjectManager;" +
                     "User:FirstName,LastName,Name;"));

                return myWorkItems;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;

            }

        }




        public static async Task<List<ProjectInsight.Models.Users.User>> GetMyTimeSheetApprovers()
        {

            try
            {
                var users = await client.GetMyTimeSheetApproversAsync(modelProperties: new ProjectInsight.Models.Base.ModelProperties("User:Name"));

                return users;


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static  async Task<List<ProjectInsight.Models.Users.User>> GetMyTimeSheetApproversAsync()
        {
            try
            {
                var users = await client.GetMyTimeSheetApproversAsync();
                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<List<ProjectInsight.Models.Users.User>> ListCommunicateActiveAsync()
        {
            try
            {
                var users = await client.ListCommunicateActiveAsync(modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Id,Name,PhotoUrl,AvatarHtml"));
                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ProjectInsight.Models.Users.User GetMe()
        {
            
            try
            {
                //return client.GetMe(new ProjectInsight.Models.Base.ModelProperties("default,Name,EmailAddress,Address1,Address2,Phone,PhotoUrl,CellPhone,City,PostCode,Country,Region,Company,Department;Department:default"));
                return client.GetMe(new ProjectInsight.Models.Base.ModelProperties("default,Name,EmailAddress,Address1,Address2,Phone,PhotoUrl,CellPhone,City,PostCode,Country,Region,AvatarHtml,AvatarHtmlAnchor,NotificationNewCount,Company;Company:default,Department;Department:default"));
                

            }
            catch (Exception ex)
            {
              //  AuthenticationService.Logout();
                return null;

            }
        }
        public static ProjectInsight.Models.Base.Responses.ApiSaveResponse SaveUser(ProjectInsight.Models.Users.User user)
        {

            try
            {
                //return client.GetMe(new ProjectInsight.Models.Base.ModelProperties("default,Name,EmailAddress,Address1,Address2,Phone,PhotoUrl,CellPhone,City,PostCode,Country,Region,Company,Department;Department:default"));
                return client.Save(user);


            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;

            }
        }
        public static ProjectInsight.Models.Users.User GetSimpleMe()
        {

            try
            {
                //return client.GetMe(new ProjectInsight.Models.Base.ModelProperties("default,Name,EmailAddress,Address1,Address2,Phone,PhotoUrl,CellPhone,City,PostCode,Country,Region,Company,Department;Department:default"));
                return client.GetMe(new ProjectInsight.Models.Base.ModelProperties("NotificationNewCount,UserGlobalCapability"));

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;

            }
        }

        public static async Task<ProjectInsight.Models.Users.User> GetUser(Guid guid)
        {
            try
            {
                return await client.GetAsync(guid, new ProjectInsight.Models.Base.ModelProperties("default,Name,EmailAddress,Address1,Address2,Phone,PhotoUrl,CellPhone,City,PostCode,Country,Region,Company,AvatarHtml,Department;Department:default"));
            }
            catch (Exception ex)
            {
               //AuthenticationService.Logout();
                return null;

            }
        }

        public static async Task<List<ProjectInsight.Models.Users.User>> ListByProjectForTaskAssignment(Guid? projectId)
        {
            try
            {
                return await client.ListByProjectForTaskAssignmentAsync(projectId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Id,Name,PhotoUrl,AvatarHtml"));
                
            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;

            }
        }

    }

}
