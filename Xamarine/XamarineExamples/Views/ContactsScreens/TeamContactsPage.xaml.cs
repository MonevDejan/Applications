using SafeSportChat.Helpers;
using SafeSportChat.ViewModels;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace SafeSportChat.Views.ContactsScreens
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamContactsPage : ContentPage
	{
        TeamContactsViewModel viewModel;
       
        public TeamContactsPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new TeamContactsViewModel();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.LoadItemsCommand.Execute(null);
        }
        private void CheckBox_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                Device.BeginInvokeOnMainThread(() => {
                    uncheckedLabel.IsVisible = false;
                    checkedLabel.IsVisible = true; 
                });
            }
            else
            {
                uncheckedLabel.IsVisible = true;
                checkedLabel.IsVisible = false;
            }
        }
        
    }
}