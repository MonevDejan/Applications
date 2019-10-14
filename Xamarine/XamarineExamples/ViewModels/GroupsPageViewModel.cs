using MvvmHelpers;
using SafeSportChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeSportChat.ViewModels
{
   
    class GroupsPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Command LoadItemsCommand { get; set; }
        public ObservableCollection<GroupName> GroupNames { get; set; }
        public GroupsPageViewModel()
        {
            Title = "Contacts";
            GroupNames = new ObservableCollection<GroupName>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }
        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                // mock data, get from api
                GroupNames.Add(new GroupName() { Name = "Adult Swim" });
                GroupNames.Add(new GroupName() { Name = "Age Group Gold" });
                GroupNames.Add(new GroupName() { Name = "Age Group Silver" });
                GroupNames.Add(new GroupName() { Name = "Beginner 1" });
                GroupNames.Add(new GroupName() { Name = "Beginner 2" });
                GroupNames.Add(new GroupName() { Name = "Junior Qualifiers" });
                GroupNames.Add(new GroupName() { Name = "Juniors Meet Coaches" });
                GroupNames.Add(new GroupName() { Name = "Seniors Meet Travel Committee" });
                GroupNames.Add(new GroupName() { Name = "Senior Gold" });
                GroupNames.Add(new GroupName() { Name = "Senior Silver" });


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
