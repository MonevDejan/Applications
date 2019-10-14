using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ProjectInsight.Models.Comments;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.Views.General;
using System.Net;
using System.IO;
using ProjectInsight.WebApi.Client.ReferenceData;
using ProjectInsight.Models.ReferenceData;
using ProjectInsight.Models.Issues;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddResolutionPage : ContentPage
    {
        IssueAddResolutionViewModel viewModel;
        public AddResolutionPage(Guid? issueId)
        {
            
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("TaskStatusPage Initialize");
            InitializeComponent();
            Title = "Add Resolution";

            viewModel = new IssueAddResolutionViewModel();
            viewModel.SelectedIssueId = issueId;
           


            BindingContext = viewModel;
          
          //  GetData();

          
        }
        //private async void GetData()
        //{

        //    viewModel.VisibleLoad = true;
        //    viewModel.LoadingMessage = "";

        //    //await GetAllUsers();
        //    viewModel.VisibleLoad = false;
          
        //}


        //private async Task<bool> GetAllUsers()
        //{
        //    try
        //    {
        //        if (SelectedUserId != null)
        //        {
        //            viewModel.Users = null;
        //            viewModel.SelectedUser = null;
        //        }

        //        List<ProjectInsight.Models.Users.User> listofUsers = await UsersService.ListByProjectForTaskAssignment(SelectedProjectId ?? Guid.Empty);
        //        ObservableCollection<ProjectInsight.Models.Users.User> users = new ObservableCollection<ProjectInsight.Models.Users.User>(listofUsers);
        //        // viewModel.Users = createInitElem(users, new ProjectInsight.Models.Users.User());
        //        viewModel.Users = users;

        //        if (SelectedUserId != null)
        //            viewModel.SelectedUser = viewModel.Users.Where(x => x.Id == SelectedUserId).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        //AuthenticationService.Logout();
        //        return false;
        //    }
        //    return true;
        //}
        //private void CmbUser_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        //{

        //    Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
        //    if (cmb.SelectedValue == null)
        //    {
        //        viewModel.SelectedUser = null;
        //    }

        //}
        private async void OnUpdateStatus(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {

                if (txtCommentBody.Text != null && txtCommentBody.Text.Trim() != string.Empty)
                {
                    try
                    {
                        var respComment = await IssuesService.client.AddResolutionAsync(viewModel.SelectedIssueId.Value, txtCommentBody.Text);
                        if (respComment.HasErrors)
                            Common.Instance.ShowToastMessage(respComment.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                    catch (Exception ex)
                    {
                        Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                    }



                    Common.RefreshWorkList = true;
                    Common.Instance.ShowToastMessage("Issue Resolution added!", DoubleResources.SuccessSnackBar);
                    MessagingCenter.Send(this, "IssueResolutionAdded", viewModel.SelectedIssueId.Value);
                    await Navigation.PopAsync();
                }
                else
                {
                    sfComment.HasError = true;
                }

            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.LoadingMessage = "";
            viewModel.VisibleLoad = false;

            //await Navigation.PushAsync(new TaskDetailsPage(viewModel.SelectedTaskId.Value));
        }

        private async void OnCancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}