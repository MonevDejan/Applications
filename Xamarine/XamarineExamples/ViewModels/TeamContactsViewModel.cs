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
    public class TeamContactsViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<SafeSportChat.Helpers.Grouping<string, Contact>> PersonsGrouped { get; set; }

        public TeamContactsViewModel()
        {
            Title = "Contacts";

            PersonsGrouped = new ObservableCollection<SafeSportChat.Helpers.Grouping<string, Contact>>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                // mock data
                List<Contact> persons = new List<Contact>();
                persons.Add(new Contact() { LastName = "Alexander", FirstName = "Bill" });
                persons.Add(new Contact() { LastName = "Allen", FirstName = "Lance" });
                persons.Add(new Contact() { LastName = "Anderson", FirstName = "Vanessa" });
                persons.Add(new Contact() { LastName = "Andrews", FirstName = "Connor" });
                persons.Add(new Contact() { LastName = "Armerding", FirstName = "Lisa" });
                persons.Add(new Contact() { LastName = "Barry", FirstName = "Sandy" });
                persons.Add(new Contact() { LastName = "Babcock", FirstName = "Mary Jo" });
                persons.Add(new Contact() { LastName = "Bechert", FirstName = "Laura" });
                persons.Add(new Contact() { LastName = "Billings", FirstName = "James" });
                persons.Add(new Contact() { LastName = "Boninga", FirstName = "Josh" });
                persons.Add(new Contact() { LastName = "Bowman", FirstName = "Bob" });
                persons.Add(new Contact() { LastName = "Canda", FirstName = "Jill" });

                var sorted = from p in persons
                             orderby p.LastName
                             group p by p.NameSort into personGroup
                             select new SafeSportChat.Helpers.Grouping<string, Contact>(personGroup.Key, personGroup);

                //create a new collection of groups


                PersonsGrouped = new ObservableCollection<SafeSportChat.Helpers.Grouping<string, Contact>>(sorted);
                base.OnPropertyChanged("PersonsGrouped");




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
