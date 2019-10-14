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

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestCombo : ContentPage
    {



        ComboTestViewModel viewModel;

        public TestCombo()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();


            viewModel = new ComboTestViewModel();
            List<Country> obsProjects = new List<Country>();
            obsProjects.Add(new Country() { Id = 0, Name = "United States", Flag = "flag1", Language = "English" });
            obsProjects.Add(new Country() { Id = 1, Name = "Macedonia", Flag = "flag2", Language = "Macedonian" });
            obsProjects.Add(new Country() { Id = 2, Name = "Canada", Flag = "flag3", Language = "English" });

            ObservableCollection<Country> countries = new ObservableCollection<Country>(obsProjects);
            viewModel.Countries = countries;
            int selectedProjectId = 2;

            viewModel.SelectedCountry = viewModel.Countries.Where(x => x.Id == selectedProjectId).FirstOrDefault();

            BindingContext = viewModel;

        }


        private void CmbCountry_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
        }


    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Language { get; set; }

    }

    public class ComboTestViewModel : BaseViewModel
    {
        public ComboTestViewModel()
        {
        }

        ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries
        {
            set
            {
                if (countries != value)
                {
                    countries = value;
                    OnPropertyChanged("Countries");
                }
            }
            get
            {
                return countries;
            }
        }

        Country selectedCountry;
        public Country SelectedCountry
        {
            set
            {
                if (selectedCountry != value)
                {
                    selectedCountry = value;

                    OnPropertyChanged("SelectedCountry");
                }
            }
            get
            {
                return selectedCountry;
            }
        }

    }


}