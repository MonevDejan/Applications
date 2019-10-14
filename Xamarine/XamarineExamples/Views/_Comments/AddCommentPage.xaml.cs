using ProjectInsight.Models.Comments;
using ProjectInsight.Models.ReferenceData;
using ProjectInsight.Models.Users;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCommentPage : ContentPage
    {
        public Guid parentItemId;
        public string parentItemName;
        public AddCommentPage (Guid parentItemId, string parentItemName)
		{
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("AddCommentPage Initialize");

            this.parentItemId = parentItemId;
            this.parentItemName = parentItemName;

            InitializeComponent ();

            txtProjectName.Text = this.parentItemName; 
        }


        public async void onCreateNewComment(object sender, EventArgs e)
        {
            var commentBody = txtCommentBody.Text;
            if (commentBody == null || commentBody.Length == 0)
            {

                Common.Instance.ShowToastMessage("Save failed because(s): A comment is required!", DoubleResources.DangerSnackBar);
            }
            else
            {
                progressBar.IsRunning = true;
                loadingContent.IsVisible = true;

                Comment comment = new Comment();
                comment.CommentBody = txtCommentBody.Text;// "Comment Added from Mobile Application";
                comment.CreatedDateTimeUTC = DateTime.UtcNow;
                comment.UserCreated_Id = Common.CurrentWorkspace.UserID;
                comment.ObjectId = parentItemId; //new Guid("8680da19-ee5b-4247-b9bb-cede65136c44");

                // var com = await CommentsService.Get(new Guid("3c9195e9-272a-48f5-8b9a-5029e82d2a7c"));

                try
                {
                    var response = await CommentsService.AddComment(comment);

                    if (!response.HasErrors)
                    {
                        Common.Instance.ShowToastMessage("Success", DoubleResources.SuccessSnackBar);
                        await Navigation.PopAsync();
                    }
                    else
                        Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                }
                catch (Exception ex)
                {
                    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                }

                progressBar.IsRunning = false;
                loadingContent.IsVisible = false;
            }
        }

        public async void onCancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}