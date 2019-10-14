using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Linq;

namespace ProjectInsightMobile.Views.TimeEntries
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActualHours : ContentPage
    {

        TimeEntryWizardViewModel viewModel;
        public ActualHours(TimeEntryWizardViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");

            viewModel = ViewModel;
            InitializeComponent();
            BindingContext = viewModel;

            Title = "Add Time Entry";
        }
        private async void Continue_Tapped(object sender, EventArgs e)
        {
            if (!viewModel.ShowContinueOnActualTimePage)
            {
                sfActualHours.HasError = true;
                sfActualHoursDec.HasError = true;

            }
            else
            {
                sfActualHours.HasError = false;
                sfActualHoursDec.HasError = false;
                if ((!string.IsNullOrEmpty(viewModel.ActualHours) && viewModel.ActualHours != "0" && viewModel.ActualHours != "00:00")
                    || (viewModel.ActualHoursDec != null && viewModel.ActualHoursDec != "0"))
                {
                    await Navigation.PushAsync(new Date(viewModel));
                }
            }
        }

        int restrictCount = 7;
        void ActualHours_OnTextChanged(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            String val = entry.Text;
            if (val != null &&  val.Length > 0)
            {
                string old_val = val.Remove(val.Length - 1);
                char lastChar = val[val.Length - 1];
                if (
                    !(Char.IsDigit(lastChar) || (lastChar == '.' && !old_val.Contains('.') && !old_val.Contains(':')) || (lastChar == ':' && !old_val.Contains('.') && !old_val.Contains(':')))
                    || val.Length > restrictCount
                    )
                {
                    entry.Text = old_val; //Set the Old value
                }
            }
            if (!viewModel.ShowContinueOnActualTimePage)
            {
                sfActualHours.HasError = true;
                sfActualHoursDec.HasError = true;
            }
            else
            {
                sfActualHours.HasError = false;
                sfActualHoursDec.HasError = false;
            }

        }
        void ActualTimey_Unfocussed(object sender, TextChangedEventArgs e)
        {
            //var oldText = e.OldTextValue;
            //var newText = e.NewTextValue.ToString();

            if (viewModel == null || string.IsNullOrEmpty(viewModel.ActualHours)) return;

            string oldValue = viewModel.ActualHours;



            if (oldValue.Contains(":"))
            {
                char splitter = ':';
                viewModel.ActualHours = TransformTime(oldValue, splitter);
            }
            else if (oldValue.Contains("."))
            {
                char splitter = '.';
                viewModel.ActualHours = TransformTime(oldValue, splitter);
            }
            else
            {
                int hours = 0;
                int.TryParse(oldValue, out hours);
                viewModel.ActualHours = hours.ToString().PadLeft(2, '0') + ":00";
            }
            if (viewModel.ActualHours == "00:00")
                viewModel.ActualHours = null;

        }
        private static string TransformTime(string oldValue, char splitter, int nearestSegment = 0, string roundDirection = "1")
        {
            int hours = 0;
            int minutes = 0;
            if (splitter == '.')
            {
                Double timeDbl = 0;
                Double.TryParse(oldValue, out timeDbl);
                TimeSpan timespan = TimeSpan.FromHours(timeDbl);
                oldValue = timespan.ToString("h\\:mm");
                splitter = ':';
            }
            var parts = oldValue.Split(splitter);
            int.TryParse(parts[0], out hours);
            if (parts.Length > 1)
            {
                int.TryParse(parts[1], out minutes);
            }

            TimeSpan ts = TimeSpan.FromMinutes(minutes);
            hours += ts.Hours;
            minutes = ts.Minutes;


            var factor = nearestSegment;
            int minRounded = minutes;
            if (factor > 0)
            {
                if (roundDirection == "1" || roundDirection.ToString().ToLower() == "closest")
                {
                    decimal dMin = ((decimal)minutes / factor);
                    minRounded = Convert.ToInt16(Math.Round(dMin, 0) * factor);
                }
                else if (roundDirection == "2" || roundDirection.ToString().ToLower() == "up")
                {
                    decimal dMin = ((decimal)minutes / factor);
                    minRounded = Convert.ToInt16(Math.Ceiling(dMin) * factor);
                }
                else if (roundDirection == "3" || roundDirection.ToString().ToLower() == "down")
                {
                    decimal dMin = ((decimal)minutes / factor);
                    minRounded = Convert.ToInt16(Math.Floor(dMin) * factor);
                }
            }


            return hours.ToString().PadLeft(2, '0') + ":" + minRounded.ToString().PadLeft(2, '0');
        }



        private void NumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            if (!viewModel.ShowContinueOnActualTimePage)
            {
                sfActualHours.HasError = true;
                sfActualHoursDec.HasError = true;
            }
            else
            {
                sfActualHours.HasError = false;
                sfActualHoursDec.HasError = false;
            }
        }

        private void ActualHours_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.NewTextValue))
                viewModel.ActualTimeError = false;

        }

        private void NumericTextBox_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel != null)
            {
                
                if (!string.IsNullOrEmpty(viewModel.ActualHoursDec) && Convert.ToDecimal(viewModel.ActualHoursDec) == 0)
                    viewModel.ActualHoursDec = null;
            }
        }

        private void IsBillable_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
          //  viewModel.IsBillable = e.IsChecked.Value;

        }
    }
}