using SafeSportChat.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using SQLite;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using System.Collections.ObjectModel;

namespace SafeSportChat.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompleteYourProfileSportPage : ContentPage
    {



        SportViewModel viewModel;

        public CompleteYourProfileSportPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();


            viewModel = new SportViewModel();
            List<Sport> obsProjects = new List<Sport>();
            obsProjects.Add(new Sport() { Id = 0, Name = "Baseball", Flag = "flag1", Language = "English" });
            obsProjects.Add(new Sport() { Id = 1, Name = "Basketball", Flag = "flag2", Language = "Macedonian" });
            obsProjects.Add(new Sport() { Id = 2, Name = "Bowling ", Flag = "flag3", Language = "English" });
            obsProjects.Add(new Sport() { Id = 3, Name = "Cheerleading", Flag = "flag2", Language = "Macedonian" });
            obsProjects.Add(new Sport() { Id = 4, Name = "Field Hockey  ", Flag = "flag3", Language = "English" });
            obsProjects.Add(new Sport() { Id = 5, Name = "Football", Flag = "flag2", Language = "Macedonian" });
            obsProjects.Add(new Sport() { Id = 6, Name = "Golf ", Flag = "flag3", Language = "English" });

            ObservableCollection<Sport> sports = new ObservableCollection<Sport>(obsProjects);
            viewModel.Sports = sports;
            int selectedProjectId = 2;

            viewModel.SelectedSport = viewModel.Sports.Where(x => x.Id == selectedProjectId).FirstOrDefault();

            BindingContext = viewModel;

        }
        private async void NoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompleteYourProfileTeamPage());
       }

        private void CmbSport_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
        }


    }

    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Language { get; set; }

    }

    public class SportViewModel : BaseViewModel
    {
        public SportViewModel()
        {
        }

        ObservableCollection<Sport> sports;
        public ObservableCollection<Sport> Sports
        {
            set
            {
                if (sports != value)
                {
                    sports = value;
                    OnPropertyChanged("Sports");
                }
            }
            get
            {
                return sports;
            }
        }

        Sport selectedSport;
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

    }


}


//using SafeSportChat.Views.Login;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Xamarin.Forms;
//using Xamarin.Forms.Xaml;
//using ProjectInsightMobile.Helpers;
//using ProjectInsightMobile.Services;
//using SQLite;
//using ProjectInsightMobile.Models;
//using ProjectInsightMobile.ViewModels;
//using System.Collections.ObjectModel;
//using ProjectInsightMobile.Views;

//namespace SafeSportChat.Views.Login
//{
//	[XamlCompilation(XamlCompilationOptions.Compile)]
//	public partial class CompleteYourProfileSportPage : ContentPage
//	{
//        SportsViewModel viewModel;

//        public CompleteYourProfileSportPage()
//        {
//            NavigationPage.SetBackButtonTitle(this, "");
//            InitializeComponent();


//            viewModel = new SportsViewModel();
//            List<Sport> obsProjects = new List<Sport>();
//            obsProjects.Add(new Sport() { Id = 0, SportName = "Baseball" });
//            obsProjects.Add(new Sport() { Id = 1, SportName = "Basketball" });
//            obsProjects.Add(new Sport() { Id = 2, SportName = "Bowling " });
//            obsProjects.Add(new Sport() { Id = 3, SportName = "Baseball" });
//            obsProjects.Add(new Sport() { Id = 4, SportName = "Basketball" });
//            obsProjects.Add(new Sport() { Id = 5, SportName = "Bowling " });
//            ObservableCollection<Sport> sports = new ObservableCollection<Sport>(obsProjects);
//            viewModel.Sports = sports;
//            int selectedProjectId = 2;

//            viewModel.SelectedSport = viewModel.Sports.Where(x => x.Id == selectedProjectId).FirstOrDefault();

//            BindingContext = viewModel;

//        }



//        private async void NoButton_Clicked(object sender, EventArgs e)
//        {
//            await Navigation.PushAsync(new TestCombo());
//        }

//        private void CmbSport_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
//        {
//            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
//        }


//    }

//    public class Sport
//    {
//        public int Id { get; set; }
//        public string SportName { get; set; }
//        //public string Flag { get; set; }
//        //public string Language { get; set; }

//    }

//    public class SportsViewModel : BaseViewModel
//    {
//        public SportsViewModel()
//        {
//        }

//        ObservableCollection<Sport> sports;
//        public ObservableCollection<Sport> Sports
//        {
//            set
//            {
//                if (sports != value)
//                {
//                    sports = value;
//                    OnPropertyChanged("Sports");
//                }
//            }
//            get
//            {
//                return sports;
//            }
//        }

//        Sport selectedSport;
//        public Sport SelectedSport
//        {
//            set
//            {
//                if (selectedSport != value)
//                {
//                    selectedSport = value;

//                    OnPropertyChanged("SelectedSport");
//                }
//            }
//            get
//            {
//                return selectedSport;
//            }
//        }

//    }

//}
