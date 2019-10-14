using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.Login
{
    public class ChoosePasswordViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ChoosePasswordViewModel()
        {
            isValid = false;  
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
    }





    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChoosePasswordPage : ContentPage
	{
        ChoosePasswordViewModel viewModel;


        public ChoosePasswordPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new ChoosePasswordViewModel();

        }


        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountAgePage());
        }


        public void ShowPass(object sender, EventArgs args)
        {
            txtPassword.IsPassword = txtPassword.IsPassword ? false : true;
        }

        private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text.Length > 7)
            {
                viewModel.isValid = true;
            }
            else
            {
                viewModel.isValid = false;
            }            
        }




      
    }
}