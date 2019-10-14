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
    public class AddSomeoneFromATeamViewModel:BaseViewModel, INotifyPropertyChanged
    {
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<State> States { get; set; }
        public ObservableCollection<Team> Teams { get; set; }

        private State selectedState;
        public State SelectedState
        {
            set
            {
                if (selectedState != value)
                {
                    selectedState = value;
                    OnPropertyChanged("SelectedState");
                }
            }
            get
            {
                return selectedState;
            }
        }


        private Team selectedTeam;
        public Team SelectedTeam
        {
            set
            {
                if (selectedTeam != value)
                {
                    selectedTeam = value;
                    OnPropertyChanged("SelectedTeam");
                }
            }
            get
            {
                return selectedTeam;
            }
        }





        public AddSomeoneFromATeamViewModel()
        {
            Title = "Complete Your Profile Team";

            States = new ObservableCollection<State>();
            Teams = new ObservableCollection<Team>();


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
                States.Add(new State() { Id = 1, Name = "CA" });
                States.Add(new State() { Id = 2, Name = "NY" });
                States.Add(new State() { Id = 3, Name = "FL" });
                States.Add(new State() { Id = 4, Name = "IL" });
                States.Add(new State() { Id = 5, Name = "AL" });

                Teams.Add(new Team() { Id = 1, Name = "Lakers" });
                Teams.Add(new Team() { Id = 2, Name = "Browns" });
                Teams.Add(new Team() { Id = 3, Name = "Rams" });
                Teams.Add(new Team() { Id = 4, Name = "etc1" });
                Teams.Add(new Team() { Id = 5, Name = "etc2" });


                //base.OnPropertyChanged("States");
                //base.OnPropertyChanged("Teams");


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

