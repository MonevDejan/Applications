using ProjectInsight.WebApi.Client;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectInsightMobile.Helpers
{
    static class APIsInitialization
    {
        public static bool isInitialized = false;

        public static bool InitializeApis(string token = null)
        {
            try
            {
                string Token = string.Empty;
                if (token == null)
                    Token = Common.CurrentWorkspace.ApiToken;
                else
                    Token = token;

                AuthenticationService.PI_Client = new ProjectInsightWebApiClient(Common.WorkspaceApi, Token);

                TasksService.client = AuthenticationService.PI_Client.Task;
                TasksService.client.ProjectInsightWebApiClient.ApiToken = Token;

                IssuesService.client = AuthenticationService.PI_Client.Issue;
                IssuesService.client.ProjectInsightWebApiClient.ApiToken = Token;

                UsersService.client = AuthenticationService.PI_Client.User;
                UsersService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ToDoService.client = AuthenticationService.PI_Client.ToDo;
                ToDoService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ItemUtilityService.client = AuthenticationService.PI_Client.ItemUtilityClient;
                ItemUtilityService.client.ProjectInsightWebApiClient.ApiToken = Token;

                CommentsService.client = AuthenticationService.PI_Client.Comment;
                CommentsService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ApprovalRequestService.client = AuthenticationService.PI_Client.ApprovalRequest;
                ApprovalRequestService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ApprovalRequestService.clientItem = AuthenticationService.PI_Client.ApprovalRequestItem;
                ApprovalRequestService.clientItem.ProjectInsightWebApiClient.ApiToken = Token;

                ApprovalRequestApprovalService.client = AuthenticationService.PI_Client.ApprovalRequestApproval;
                ApprovalRequestApprovalService.client.ProjectInsightWebApiClient.ApiToken = Token;

                NotificationsService.client = AuthenticationService.PI_Client.Notification;
                NotificationsService.client.ProjectInsightWebApiClient.ApiToken = Token;

                TimeEntryService.client = AuthenticationService.PI_Client.TimeEntry;
                TimeEntryService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ExpenseCodeService.client = AuthenticationService.PI_Client.ExpenseCode;
                ExpenseCodeService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ProjectsService.client = AuthenticationService.PI_Client.Project;
                ProjectsService.client.ProjectInsightWebApiClient.ApiToken = Token;

                DocumentsService.clientFiles = AuthenticationService.PI_Client.FileItem;
                DocumentsService.clientFiles.ProjectInsightWebApiClient.ApiToken = Token;

                DocumentsService.clientFolders = AuthenticationService.PI_Client.Folder;
                DocumentsService.clientFolders.ProjectInsightWebApiClient.ApiToken = Token;

                TimeSheetService.client = AuthenticationService.PI_Client.TimeSheet;
                TimeSheetService.client.ProjectInsightWebApiClient.ApiToken = Token;

                WorkPercentCompleteTypeService.client = AuthenticationService.PI_Client.WorkPercentCompleteType;
                WorkPercentCompleteTypeService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ProjectStatusService.client = AuthenticationService.PI_Client.ProjectStatus;
                ProjectStatusService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ProjectTypeService.client = AuthenticationService.PI_Client.ProjectType;
                ProjectTypeService.client.ProjectInsightWebApiClient.ApiToken = Token;


                WorkspaceService.client = AuthenticationService.PI_Client.WorkspaceClient;
                WorkspaceService.client.ProjectInsightWebApiClient.ApiToken = Token;

                SearchesService.client = AuthenticationService.PI_Client.Search;
                SearchesService.client.ProjectInsightWebApiClient.ApiToken = Token;

                FileItemService.client = AuthenticationService.PI_Client.FileItem;
                FileItemService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ExpenseReportService.client = AuthenticationService.PI_Client.ExpenseReport;
                ExpenseReportService.client.ProjectInsightWebApiClient.ApiToken = Token;

                ExpenseEntryService.client = AuthenticationService.PI_Client.ExpenseEntry;
                ExpenseEntryService.client.ProjectInsightWebApiClient.ApiToken = Token;

                CompanyService.client = AuthenticationService.PI_Client.Company;
                CompanyService.client.ProjectInsightWebApiClient.ApiToken = Token;


                IssueStatusTypeService.client = AuthenticationService.PI_Client.IssueStatusType;
                IssueStatusTypeService.client.ProjectInsightWebApiClient.ApiToken = Token;



                IssueTypeService.client = AuthenticationService.PI_Client.IssueType;
                IssueTypeService.client.ProjectInsightWebApiClient.ApiToken = Token;

                IssueSeverityTypeService.client = AuthenticationService.PI_Client.IssueSeverityType;
                IssueSeverityTypeService.client.ProjectInsightWebApiClient.ApiToken = Token;

                IssuePriorityTypeService.client = AuthenticationService.PI_Client.IssuePriorityType;
                IssuePriorityTypeService.client.ProjectInsightWebApiClient.ApiToken = Token;

                PushNotifications.ConfigurePushNotificationSettings();

                //Common.Instance.SendPushNotificationToken();

                isInitialized = true;
                return isInitialized;
            }
            catch (Exception ex)
            {
                isInitialized = false;
                return isInitialized;
            }
        }
    }
}
