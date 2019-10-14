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
    public class InviteParentViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public InviteParentViewModel()
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
	public partial class InviteParentPage : ContentPage
    {
        InviteParentViewModel viewModel;

        public InviteParentPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new InviteParentViewModel();
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompleteYourProfileSportPage());
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