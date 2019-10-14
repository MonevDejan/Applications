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
    public class GeneralContactViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<Contact> AllPersons { get; set; }
        public ObservableCollection<SafeSportChat.Helpers.Grouping<string, Contact>> FilteredPersonsGrouped { get; set; }

        public GeneralContactViewModel()
        {
            Title = "Contacts";

            AllPersons = new ObservableCollection<Contact>();
            FilteredPersonsGrouped = new ObservableCollection<SafeSportChat.Helpers.Grouping<string, Contact>>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        string filter = String.Empty;
        public string Filter
        {
            get => filter;
            set
            {
                if (SetProperty(ref filter, value.ToLower()))
                    FilterItems();
            }
        }

        void FilterItems()
        {
            var sorted = from p in AllPersons
                         where p.LastName.ToLower().Contains(Filter) || p.FirstName.ToLower().Contains(Filter)
                         orderby p.LastName
                         group p by p.NameSort into personGroup
                         select new SafeSportChat.Helpers.Grouping<string, Contact>(personGroup.Key, personGroup);

            //create a new collection of groups
            FilteredPersonsGrouped = new ObservableCollection<SafeSportChat.Helpers.Grouping<string, Contact>>(sorted);
            base.OnPropertyChanged("FilteredPersonsGrouped");
        }



        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                // mock data, get from api
                AllPersons.Add(new Contact() { LastName = "Alexander", FirstName = "Bill" });
                AllPersons.Add(new Contact() { LastName = "Allen", FirstName = "Lance" });
                AllPersons.Add(new Contact() { LastName = "Anderson", FirstName = "Vanessa" });
                AllPersons.Add(new Contact() { LastName = "Andrews", FirstName = "Connor" });
                AllPersons.Add(new Contact() { LastName = "Armerding", FirstName = "Lisa" });
                AllPersons.Add(new Contact() { LastName = "Barry", FirstName = "Sandy" });
                AllPersons.Add(new Contact() { LastName = "Babcock", FirstName = "Mary Jo" });
                AllPersons.Add(new Contact() { LastName = "Bechert", FirstName = "Laura" });
                AllPersons.Add(new Contact() { LastName = "Billings", FirstName = "James" });
                AllPersons.Add(new Contact() { LastName = "Boninga", FirstName = "Josh" });
                AllPersons.Add(new Contact() { LastName = "Bowman", FirstName = "Bob" });
                AllPersons.Add(new Contact() { LastName = "Canda", FirstName = "Jill" });

                FilterItems();

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
