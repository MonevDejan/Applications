using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.Login
{

    public class ConfirmPhoneViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ConfirmPhoneViewModel()
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
	public partial class CreateAccountByPhoneNumberConfirmCode : ContentPage
	{

        ConfirmPhoneViewModel viewModel;


        public CreateAccountByPhoneNumberConfirmCode ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new ConfirmPhoneViewModel();

        }
        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChoosePasswordPage());
        }

        private void TxtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCode.Text.Length > 5) // todo add regex
            {
                viewModel.isValid = true;
            }
            else
            {
                viewModel.isValid = false;
            }
        }

        private void ResendButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Thank you", "New activation code has been send to your phone", "OK");
        }
    }
}