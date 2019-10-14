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

    public class CreateTeamViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public CreateTeamViewModel()
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
	public partial class CreateTeamPage : ContentPage
	{
       CreateTeamViewModel viewModel;

        public CreateTeamPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new CreateTeamViewModel();
        }

        private async void InviteHeadCoachButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InviteHeadCoachPage());
        }

        private void entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        public void Validate()
        {
            viewModel.isValid = false;

            if (txtFullTeamName.Text != null && txtFullTeamName.Text.Length > 5)
            {
                if (txtTeamAbbreviation.Text != null && txtTeamAbbreviation.Text.Length > 5)
                {
                    if (txtCity.Text != null && txtCity.Text.Length > 5)
                    {
                       
                        if (txtState.Text != null && txtState.Text.Length > 5)
                        {
                            viewModel.isValid = true;
                        }
                    }
                }
            }
        }
           



        //private void TxtFullTeamName_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (txtFullTeamName.Text.Length > 10) // todo add regex for phones
        //    {
        //        if (txtTeamAbbreviation.Text.Length > 10) // todo add regex for phones
        //        {
        //            if (txtCity.Text.Length > 10) // todo add regex for phones
        //            {
        //                if (txtState.Text.Length > 10) // todo add regex for phones
        //                {
        //                    viewModel.isValid = true;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        viewModel.isValid = false;
        //    }
        //}
    }
}