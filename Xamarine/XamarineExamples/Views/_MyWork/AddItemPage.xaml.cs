using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Services;
using System.Collections.ObjectModel;
using static ProjectInsightMobile.ViewModels.NewProjectViewModel;
using System.Linq;
using ProjectInsight.Models.Base.Responses;
using Plugin.FilePicker;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddItemPage : ContentPage
    {


        Guid? ParentId;
        public AddItemPage(Guid? parentId = null)
        { 
            InitializeComponent();

            ParentId = parentId;
            //Check users permissions for adding Tasks, Projects, ToDos
            ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();

            //if (userMe != null && userMe.UserGlobalCapability != null && userMe.UserGlobalCapability.CanCreateProjects)
            if(Common.UserGlobalCapability != null && Common.UserGlobalCapability.CanCreateProjects && parentId == null)
            {
                slProject.IsVisible = true;
            }

            if (parentId == null)
            {
                slApproval.IsVisible = true;
                slTodos.IsVisible = true;
            }
        }



        private async void AddTask_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage(parentId: ParentId));
        }
        private async void AddProject_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewProjectPage());
        }

        private async void AddToDo_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewToDoPage());

        }
        private async void AddIssue_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewIssuePage(parentId: ParentId));
        }

        

        private async void AddApproval_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_WhatToApprove());

        }
        
    }
}