using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.Login
{

    public class ChooseEmailViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ChooseEmailViewModel()
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
	public partial class CreateAccountByEmailPage : ContentPage
	{
        ChooseEmailViewModel viewModel;


        public CreateAccountByEmailPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new ChooseEmailViewModel();

        }
        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountByEmailConfirmCode());
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                bool isEmail = Regex.IsMatch(txtEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail)
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
}