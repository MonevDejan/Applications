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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInUsingPhoneOrEmailPage : ContentPage
	{
        public class SignInUsingPhoneOrEmailViewModel : BaseViewModel, INotifyPropertyChanged
        {
            public SignInUsingPhoneOrEmailViewModel()
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

        SignInUsingPhoneOrEmailViewModel viewModel;
        public SignInUsingPhoneOrEmailPage ()
        {
			InitializeComponent ();
            BindingContext = viewModel = new SignInUsingPhoneOrEmailViewModel();
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

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogInMagicLinkPage());
        }
    }
}