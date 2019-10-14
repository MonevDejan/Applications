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

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ToDoDetailsPage : ContentPage
    {
        ToDoDetailsViewModel viewModel;
        ProjectInsight.Models.ToDos.ToDo item;
        public ToDoDetailsPage(Guid itemId)
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();
            BindingContext = viewModel = new ToDoDetailsViewModel();
            Get(itemId);
                       
        }
        private async void Get(Guid itemId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetItem(itemId);

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {                                      
                viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> GetItem(Guid itemId)
        {

            try
            {
                item = await ToDoService.GetItem(itemId);

                if (item != null)
                {
                    var itemVM = new ToDoDetailsViewModel
                    {
                        Id = item.Id.ToString(),
                        Title = item.Name,
                        ProjectName = item.ProjectAffiliation != null ? item.ProjectAffiliation.ItemNumberFullAndNameDisplayPreference : "",
                        Description = item.Description,
                        WorkStatus = item.WorkPercentCompleteType != null ? item.WorkPercentCompleteType.Name : "",
                        UserCreated = item.UserCreated,
                        UserAssignedBy = item.UserAssignedBy.FirstName + " " + item.UserAssignedBy.LastName,
                        WorkPercentComplete = item.WorkPercentComplete.Value,
                        WorkPercentCompleteType = item.WorkPercentCompleteType
                    };
                    if (item.UserAssignedTo != null)
                    {
                        itemVM.UserAssignedTo = "Assigned To: " + item.UserAssignedTo.FirstName + " " + item.UserAssignedTo.LastName;
                    }
                    if (item.UserAssignedBy != null)
                    {
                        itemVM.UserAssignedBy = "Assigned By: " + item.UserAssignedBy.FirstName + " " + item.UserAssignedBy.LastName;
                    }
                    if (item.StartDateTimeUTC != null)
                    {
                        itemVM.StartDate = item.StartDateTimeUTC.Value;
                    }
                    if (item.EndDateTimeUTC != null)
                    {
                        itemVM.EndDate = item.EndDateTimeUTC.Value;
                    }
                    //Common.Instance.ItemComments = item.Comments;
                    viewModel = itemVM;
                    BindingContext = viewModel;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        public async void OnMarkComplete(object sender, EventArgs e)
        {


            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {
                item.WorkPercentComplete = viewModel.WorkPercentComplete = 100;
                var response = await ToDoService.SaveTodo(item);

                if (!response.HasErrors)
                {
                    //btnMarkAsComplete.IsVisible = false;
                    //txtStatus.Text = viewModel.WorkStatus = "Done";
                    Common.Instance.ShowToastMessage("Success mark as completed ToDo!", DoubleResources.SuccessSnackBar);
                }
                else
                    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);


            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.LoadingMessage = "Success";
            viewModel.VisibleLoad = false;
        }   

        public async void OnUpdate(object sender, EventArgs e)
        {
                                            
        }         

        public async void OnAddComment(object sender, EventArgs e)
        {
                                                                   
        }

        private async void OnAddTimeEntry(object sender, EventArgs e)
        {


            //await Navigation.PushAsync(new CreateTimeEntryPage(task));

            TimeEntryWizardViewModel ViewModel = new TimeEntryWizardViewModel();
            ViewModel.ItemType = ItemType.Todo;
            ViewModel.SelectedTask = new TaskExt(new ProjectInsight.Models.Tasks.Task() { Id = new Guid(viewModel.Id) });
            await Navigation.PushAsync(new TimeEntries.ExpenseCode(ViewModel));

        }


        private async void OnShowDescriptionAllContent(object sender, EventArgs e)
        {
            DescriptionViewModel descVM = new DescriptionViewModel();
            descVM.Description = viewModel.Description;
            descVM.Title = viewModel.Title;
            await Navigation.PushAsync(new HtmlContentDescription(descVM));
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {

            MessagingCenter.Subscribe<NewToDoPage, Guid>(this, "TodoUpdated", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    Get(itemId);

                MessagingCenter.Unsubscribe<NewToDoPage, Guid>(this, "TodoUpdated");
            });


            await Navigation.PushAsync(new NewToDoPage(new Guid(viewModel.Id)));
        }

        private async void OnUpdateStatus(object sender, EventArgs e)
        {


            MessagingCenter.Subscribe<ToDoStatusPage, Guid>(this, "ToDoStatusUpdated", async (obj, todoId) =>
            {
                Get(todoId);

                MessagingCenter.Unsubscribe<ToDoStatusPage, Guid>(this, "ToDoStatusUpdated");
            });

            await Navigation.PushAsync(new ToDoStatusPage(new Guid(viewModel.Id), viewModel.WorkPercentCompleteType));
        }
    }
}