using SafeSportChat.Models;
using SafeSportChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompleteYourProfilePage : ContentPage
	{

        CompleteProfileViewModel viewModel;

        public CompleteYourProfilePage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new CompleteProfileViewModel();

        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new YouAreAllSetPage());
        }

        private void CmbSport_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            //viewModel.SelectedSport = (Sport)cmbSport.SelectedItem;
            Validate();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }

        private void entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        public void Validate()
        {
            viewModel.isValid = false;

            if (txtFirstName.Text != null && txtFirstName.Text.Length > 5)
            {
                if (txtLastName.Text != null && txtLastName.Text.Length > 5)
                {
                    if (txtEmail.Text != null)
                    {
                        bool isEmail = Regex.IsMatch(txtEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        if (isEmail)
                        {

                            if (txtPhone.Text != null && txtPhone.Text.Length > 10)
                            {
                                if (viewModel.SelectedSport != null)
                                {
                                    viewModel.isValid = true;
                                }
                            }
                        }
                    }

                }
            }

        }

    }
}