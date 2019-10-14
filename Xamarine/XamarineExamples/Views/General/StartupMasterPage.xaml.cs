using Plugin.Connectivity;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using SafeSportChat.Views.Login;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartupMasterPage : MasterDetailPage
    {
		public StartupMasterPage ()
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();

            //MasterPage.ListView.ItemSelected += OnItemSelected;


            if (Common.PushNotificationAction != null)
            {
                //switch workspace
                if (Common.CurrentWorkspace == null || Common.CurrentWorkspace.DomainID != Common.PushNotificationAction.DomainId)
                {
                    var workspaces = Common.Instance.GetWorkspaces();
                    Workspace ws;
                    ws = workspaces.FirstOrDefault(x => x.DomainID == Common.PushNotificationAction.DomainId);
                    if (ws != null)
                    {
                        foreach (Workspace wsp in workspaces.Where(x => x.isActive))
                        {
                            wsp.isActive = false;
                            Common.Instance._sqlconnection.Update(wsp);
                        }
                        ws.isActive = true;
                        Common.Instance._sqlconnection.Update(ws);
                        Common.CurrentWorkspace = ws;

                        if (!string.IsNullOrEmpty(ws.ApiToken) && ws.UserID != null)
                        {

                            //check if ApiToken is not expired
                            var handler = new JwtSecurityTokenHandler();
                            var tokenS = handler.ReadToken(ws.ApiToken) as JwtSecurityToken;
                            var exp = tokenS.Claims.First(claim => claim.Type == "exp").Value;

                            var expDate = tokenS.ValidTo;
                            if (expDate < DateTime.UtcNow.AddMinutes(1))
                            {

                                ws.ApiToken = null;
                                Common.Instance._sqlconnection.Update(ws);
                                Common.CurrentWorkspace = ws;
                                this.Detail = new NavigationPage(new SignInUsingPhoneOrEmailPage());

                                return;
                            }

                            //if (AuthenticationService.PI_Client == null)
                            APIsInitialization.InitializeApis();

                            ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();
                            Common.UserGlobalCapability = userMe.UserGlobalCapability;
                            Common.WorkspaceCapability = WorkspaceService.GetWorkspaceCapability();

                            string notifCount = string.Empty;
                           // Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = false;
                            if (userMe.NotificationNewCount != null && userMe.NotificationNewCount.Value > 0)
                            {
                                //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = true;
                                if (userMe.NotificationNewCount.Value > 99)
                                    notifCount = "99+";
                                else
                                    notifCount = userMe.NotificationNewCount.Value.ToString();
                            }

                            Common.Instance.bottomNavigationViewModel.NumberNottificationItems = notifCount;
                            Common.Instance.bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType != ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;
                            Common.Instance.bottomNavigationViewModel.NumberProjectItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;

                        }
                    }

                }

                //ProjectInsight.Items.Projects.Tasks.Task
                //ProjectInsight.Items.Projects.Project
                //ProjectInsight.Items.ApprovalRequests.ApprovalRequest
                //ProjectInsight.Items.Issues.Issue
                //ProjectInsight.Items.Files.File
                //ProjectInsight.Items.CustomItems.CustomItem
                //ProjectInsight.Items.Proposals.Proposal
                //ProjectInsight.Items.ToDos.ToDo
                //ProjectInsight.Items.Workflows.Workflow
                //ProjectInsight.Items.Domains.TimeAndBilling.TimeEntries.TimeSheet
                //ProjectInsightItems.Domains.TimeAndBilling.ExpenseEntries.ExpenseReport

                if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.Projects.Tasks.Task")
                    this.Detail = new NavigationPage(new TaskDetailsPage(Common.PushNotificationAction.ObjectId));

                else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.Projects.Project")
                    this.Detail = new NavigationPage(new ProjectDetailsPage(Common.PushNotificationAction.ObjectId));

                else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.ApprovalRequests.ApprovalRequest")
                    this.Detail = new NavigationPage(new ApprovalRequestPage(Common.PushNotificationAction.ObjectId));

                else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.Issues.Issue")
                    this.Detail = new NavigationPage(new IssueDetailsPage(Common.PushNotificationAction.ObjectId));

                //else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.Files.File")
                //    this.Detail = new NavigationPage(new IssueDetailsPage(Common.PushNotificationAction.ObjectId));

                //else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.CustomItems.CustomItem")
                //    this.Detail = new NavigationPage(new IssueDetailsPage(Common.PushNotificationAction.ObjectId));

                else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.ToDos.ToDo")
                    this.Detail = new NavigationPage(new ToDoDetailsPage(Common.PushNotificationAction.ObjectId));

                //else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.Workflows.Workflow")
                //    this.Detail = new NavigationPage(new ToDoDetailsPage(Common.PushNotificationAction.ObjectId));
                //else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsight.Items.Domains.TimeAndBilling.TimeEntries.TimeSheet")
                //    this.Detail = new NavigationPage(new ToDoDetailsPage(Common.PushNotificationAction.ObjectId));
                //else if (Common.PushNotificationAction.ObjectTypeString == "ProjectInsightItems.Domains.TimeAndBilling.ExpenseEntries.ExpenseReport")
                //    this.Detail = new NavigationPage(new ToDoDetailsPage(Common.PushNotificationAction.ObjectId));

                else
                    this.Detail = new NavigationPage(new NotificationsPage());

                Common.PushNotificationAction = null;
                return;
            }

        }

     
        //void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var item = e.SelectedItem as MasterPageMenuItem;
        //    if (item != null)
        //    {
        //        if (item.Id == 33)
        //        {

        //            AuthenticationService.Logout();

        //            return;

        //        }
        //        if (!CrossConnectivity.Current.IsConnected) return;

        //        if (item.Id ==44)
        //        {
        //            Common.Instance.bottomNavigationViewModel.ActiveIcon = 4; 
        //        }
                
        //        Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
        //        MasterPage.ListView.SelectedItem = null;
        //        IsPresented = false;
        //    }
        //}
    }
}