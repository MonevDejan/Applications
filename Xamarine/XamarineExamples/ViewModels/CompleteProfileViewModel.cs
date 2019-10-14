using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SafeSportChat.Models;

namespace SafeSportChat.ViewModels
{



    public class CompleteProfileViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public CompleteProfileViewModel()
        {
            Title = "Complete Your Profile";
            Sports = new ObservableCollection<Sport>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public bool _isValid { get; set; }
        public bool isValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                base.OnPropertyChanged("isValid");
            }
        }


        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<Sport> Sports { get; set; }

        private Sport selectedSport;
        public Sport SelectedSport
        {
            set
            {
                if (selectedSport != value)
                {
                    selectedSport = value;
                    OnPropertyChanged("SelectedSport");
                }
            }
            get
            {
                return selectedSport;
            }
        }



        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                // mock data, get from api
                Sports.Add(new Sport() {  Id = 1, Name = "NBA"});
                Sports.Add(new Sport() {  Id = 2, Name = "NFL"});
                Sports.Add(new Sport() {  Id = 3, Name = "MLB"});
                Sports.Add(new Sport() {  Id = 4, Name = "NHL"});


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
