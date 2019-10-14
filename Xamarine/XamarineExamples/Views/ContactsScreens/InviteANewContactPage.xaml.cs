using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmHelpers;
using Syncfusion.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views
{
    public class InviteSomeoneNewViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public InviteSomeoneNewViewModel()
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
    public partial class InviteANewContactPage : ContentPage
    {
        InviteSomeoneNewViewModel viewModel;
        public InviteANewContactPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new InviteSomeoneNewViewModel();
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