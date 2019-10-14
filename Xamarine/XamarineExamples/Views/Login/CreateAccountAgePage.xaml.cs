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
    public class ChooseAgeViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ChooseAgeViewModel()
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
	public partial class CreateAccountAgePage : ContentPage
	{
        ChooseAgeViewModel viewModel;

        public CreateAccountAgePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new ChooseAgeViewModel();

        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InviteParentPage());
        }

        private void Dtp_DateSelected(object sender, DateChangedEventArgs e)
        {
            //todo add validation
            if (DateTime.Now.Subtract(dtp.Date).TotalDays > 6 * 365)
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