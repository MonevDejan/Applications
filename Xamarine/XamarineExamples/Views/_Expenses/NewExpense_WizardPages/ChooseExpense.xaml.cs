using ProjectInsight.Models.Projects;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseExpense : ContentPage
    {
        Guid? TaskId = null;
        Guid? ProjectId = null;
        public ChooseExpense(Guid? taskId = null, Guid? projectId = null)
		{
            NavigationPage.SetBackButtonTitle(this,"");
            TaskId = taskId;
            ProjectId = projectId;
            InitializeComponent();

            if (taskId.HasValue)
                btnSupplies.IsVisible = false;
        }

        private async void Receipt_Tapped(object sender, EventArgs e)
        {
            ExpenseEntryViewModel viewModel = new ExpenseEntryViewModel() { SelectedExpenseType = 1 };
            if (TaskId != null)
            {
                viewModel.SelectedTask = (new TaskExt((new ProjectInsight.Models.Tasks.Task { Id = TaskId })));
                viewModel.AddingToTask = true;
            }
            else if (ProjectId != null)
            {
                viewModel.SelectedProject = (new Project(ProjectId.Value));
                viewModel.AddingToProject = true;
            }

            await Navigation.PushAsync(new TakePhoto(viewModel));
        }
        private async void Miles_Tapped(object sender, EventArgs e)
        {
            ExpenseEntryViewModel viewModel = new ExpenseEntryViewModel() { SelectedExpenseType = 2 };
            if (TaskId != null)
            {
                viewModel.SelectedTask = (new TaskExt((new ProjectInsight.Models.Tasks.Task { Id = TaskId })));
                viewModel.AddingToTask = true;
            }
            else if (ProjectId != null)
            {
                viewModel.SelectedProject = (new Project(ProjectId.Value));
                viewModel.AddingToProject = true;
            }
            await Navigation.PushAsync(new Distance(viewModel));

        }

       
        private async void Inventory_Tapped(object sender, EventArgs e)
        {
            ExpenseEntryViewModel viewModel = new ExpenseEntryViewModel() { SelectedExpenseType = 3 };
            if (TaskId != null)
            {
                viewModel.SelectedTask = (new TaskExt((new ProjectInsight.Models.Tasks.Task { Id = TaskId })));
                viewModel.AddingToTask = true;
                await Navigation.PushAsync(new ExpenseCode(viewModel));
            }
            else if (ProjectId != null)
            {
                viewModel.SelectedProject = (new Project(ProjectId.Value));
                viewModel.AddingToProject = true;
                await Navigation.PushAsync(new ExpenseCode(viewModel));
            }
            else
            {
               
                await Navigation.PushAsync(new Company_Project_Task2(viewModel));
            }

        }

        private async void Test_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageEditor());
        }

        private async void Supplies_Tapped(object sender, EventArgs e)
        {
            if (ProjectId == null)
            {
                await Navigation.PushAsync(new ProjectSelect());
            }
            else
            {
                await Navigation.PushAsync(new ExpensesBulkEntry("", projectId:ProjectId.Value));
            }
        }
    }
}