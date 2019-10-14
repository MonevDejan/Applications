using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Services;
using System.Collections.ObjectModel;
using ProjectInsight.Models.Base.Responses;
using System.Collections.Generic;
using static ProjectInsightMobile.ViewModels.NewApprovalViewModel;
using ProjectInsight.Models.Users;
using HtmlAgilityPack;
using ProjectInsight.Models.ApprovalRequests;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_Approvers : ContentPage
    {
      
        NewApprovalViewModel viewModel;
        SettingsService settings = new SettingsService();

        Guid pId = Guid.Empty;

        public NewApprovalPage_Approvers(NewApprovalViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();

            viewModel = ViewModel;

            StartLoading();

            BindingContext = viewModel;

            if (viewModel.Id.HasValue)
            {
                Title = "Edit Approval";
            }
        }
        private async void StartLoading()
        {
            viewModel.VisibleLoad = true;

            bool isSuccess = await GetAllUsers();

            viewModel.VisibleLoad = false;
        }

        private async Task<bool> GetAllUsers(Guid? selectedUserId = null, Guid ? selectedProjectId = null)
        {
            try
            {
                if (selectedUserId != null)
                {
                    viewModel.Approvers = null;
                    viewModel.SelectedApprovers= null;
                }

                List<User> listofUsers = await UsersService.ListCommunicateActiveAsync();
                //viewModel.Approvers = listofUsers;
                viewModel.Approvers = new ObservableCollection<Approver>();

                foreach (var user in listofUsers)
                {
                    Approver app = new Approver();
                    app.ShowAvatar = true;
                    app.ShowImage = false;
                    app.Name = "";
                    app.PhotoURL = "";
                    app.AvatarColor = "#fff";
                    app.AvatarInitials = "PI";
                    if (string.IsNullOrEmpty(user.PhotoUrl))
                    {
                        if (!string.IsNullOrEmpty(user.AvatarHtml))
                        {
                            string myDiv = user.AvatarHtml;
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(myDiv);
                            HtmlNode node = doc.DocumentNode.SelectSingleNode("div");
                            string AvatarInitials = "US";
                            string AvatarColor = "#fff";
                            string PhotoURL = string.Empty;

                            if (node != null)
                            {
                                AvatarInitials = (node.ChildNodes[0]).OuterHtml;
                                foreach (HtmlAttribute attr in node.Attributes)
                                {
                                    if (attr.Name.ToLower() == "style")
                                    {
                                        string[] parts = attr.Value.Split('#');
                                        if (parts != null && parts.Length > 1)
                                        {
                                            AvatarColor = parts[1];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                HtmlNode node2 = doc.DocumentNode.SelectSingleNode("img");
                                if (node2 != null)
                                {
                                    foreach (HtmlAttribute attr in node2.Attributes)
                                    {
                                        if (attr.Name.ToLower() == "src")
                                        {
                                            string imageUrl = attr.Value.Replace("/ProjectInsight.WebApp", "");
                                            PhotoURL = Common.CurrentWorkspace.WorkspaceURL + imageUrl;
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(PhotoURL))
                            {
                                //show stack layout
                                app.AvatarColor = "#" + AvatarColor;
                                app.AvatarInitials = AvatarInitials;
                                app.ShowAvatar = true;
                                app.ShowImage = false;
                                app.PhotoURL = "";

                            }
                            else
                            {
                                //show image control
                                app.PhotoURL = PhotoURL;
                                app.ShowAvatar = false;
                                app.ShowImage = true;
                            }



                        }
                    }
                    else
                    {
                        //show image control
                        app.PhotoURL = Common.CurrentWorkspace.WorkspaceURL + user.PhotoUrl;
                        app.ShowAvatar = false;
                        app.ShowImage = true;
                    }
                    app.Name = user.Name;
                    app.Id = user.Id.Value;
                    viewModel.Approvers.Add(app);
                }
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }
        private async void OnSave_Tapped(object sender, EventArgs e)
        {

            //selectiraniot Task e 
            //var selTask = viewModel.SelectedTask;
            bool validationError = false;
            if (viewModel.SelectedApprovers == null || viewModel.SelectedApprovers.Count == 0)
            {
                viewModel.SelectedApproversError = true;
                validationError = true;
            }

            if (validationError) return;

            viewModel.VisibleLoad = true;



            ProjectInsight.Models.ApprovalRequests.ApprovalRequest approval = new ProjectInsight.Models.ApprovalRequests.ApprovalRequest();
            approval.Name = viewModel.Name;
            approval.Description = viewModel.Description;
            //approval.UserCreated_Id = Common.CurrentWorkspace.UserID;
            //approval.Id = viewModel.Id;
            approval.AreAllApproversRequired = viewModel.AreAllApproversRequired;
            approval.IsSequentialApproval = viewModel.IsSequentialApproval;
            approval.ApprovalRequestDateTimeUTC = DateTime.UtcNow;
            approval.ApprovalRequestStateType = 0;
            approval.ItemContainer_Id = viewModel.Container_Id;



            if (viewModel.Container_Id.HasValue && viewModel.Container_Id != null)
            {
                approval.ApprovalRequestItems = new List<ApprovalRequestItem>();
                ApprovalRequestItem item = new ApprovalRequestItem();
               // item.ApprovalRequest_Id = resp.SavedId;
                item.Item_Id = viewModel.Container_Id;
                approval.ApprovalRequestItems.Add(item);
            }

            int i = 0;
            if (viewModel.SelectedApprovers != null && viewModel.SelectedApprovers.Count > 0)
            {
                approval.ApprovalRequestApprovals = new List<ApprovalRequestApproval>();
                foreach (var approver in viewModel.SelectedApprovers)
                {
                    ApprovalRequestApproval app = new ApprovalRequestApproval();
                    app.Approver_Id = approver.Id;
                    app.ApprovalOrder = i;
                    //app.ApprovalRequest_Id = resp.SavedId;
                    app.ApprovalRequestApprovalStateType = 0;
                    app.Comment = "";
                    i++;
                    approval.ApprovalRequestApprovals.Add(app);
                }
            }

                ApiSaveResponse resp = await ApprovalRequestService.Save(approval);
            

            if (resp == null)
            {
                Common.Instance.ShowToastMessage("Error,check again.", DoubleResources.DangerSnackBar);
            }
            else if (resp.HasErrors)
            {
                Common.Instance.ShowToastMessage(resp.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
            }
            else if (!resp.HasErrors)
            {

                //if (viewModel.Container_Id.HasValue && viewModel.Container_Id != null)
                //{
                //    ApprovalRequestItem item = new ApprovalRequestItem();
                //    item.ApprovalRequest_Id = resp.SavedId;
                //    item.Item_Id = viewModel.Container_Id;

                //    try
                //    {
                //        ApiSaveResponse respItem = await ApprovalRequestService.clientItem.SaveAsync(item);
                //        if (respItem == null)
                //        {
                //            Common.Instance.ShowToastMessage("Error,check again.", DoubleResources.DangerSnackBar);
                //        }
                //        else if (respItem.HasErrors)
                //        {
                //            Common.Instance.ShowToastMessage(respItem.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //Common.Instance.ShowToastMessage("Approval Saved", DoubleResources.SuccessSnackBar);
                Common.RefreshWorkList = true;
                await Navigation.PushAsync(new NewApprovalPage_Congratulations());
            }
            viewModel.VisibleLoad = false;
        }

        private async void OnCancel_Tapped(object sender,DateChangedEventArgs e)
        {

            await Navigation.PopAsync();
        }

       


        void Handle_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            List<Approver> items = new List<Approver>();
            if (e.Value != null && (((e.Value is string && (string)e.Value != string.Empty)) || (e.Value is System.Collections.IList && (e.Value as System.Collections.IList).Count > 0)))
            {

                    for (int ii = 0; ii < (e.Value as List<object>).Count; ii++)
                    {
                        var collection = (e.Value as List<object>)[ii];
                        items.Add((Approver)collection);
                    }
                
                viewModel.SelectedApprovers = items;
                if (items.Count > 0)
                {
                    //listView.SeparatorColor = Color.Black;
                }
                else
                {
                    //listView.SeparatorColor = Color.Transparent;
                }
            }
            else
            {
                viewModel.SelectedApprovers = null;
            }

        }

    

    }
}