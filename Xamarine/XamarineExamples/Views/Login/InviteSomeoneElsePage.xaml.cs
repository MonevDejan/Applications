using MvvmHelpers;
using ProjectInsightMobile.Views;
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

    public class InviteSomeoneElseViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public InviteSomeoneElseViewModel()
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
	public partial class InviteSomeoneElsePage : ContentPage
	{
        InviteSomeoneElseViewModel viewModel;
        public InviteSomeoneElsePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new InviteSomeoneElseViewModel();
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

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartUp());
        }
    }
}