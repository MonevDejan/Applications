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
    public class ChoosePhoneViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ChoosePhoneViewModel()
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
	public partial class CreateAccountByPhoneNumberPage : ContentPage
	{
        ChoosePhoneViewModel viewModel;

        public CreateAccountByPhoneNumberPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new ChoosePhoneViewModel();
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountByPhoneNumberConfirmCode());
        }

        private void TxtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPhone.Text.Length > 10) // todo add regex for phones
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