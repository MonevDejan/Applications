using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.General
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FullScreenVideoPlayerPage : ContentPage
	{
		public FullScreenVideoPlayerPage ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            videoPlayer.Play();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            //if (Device.RuntimePlatform == Device.WinPhone)
            //    videoPlayer.IsVisible = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            videoPlayer.Pause();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            //if (Device.RuntimePlatform == Device.WinPhone)
            //    videoPlayer.IsVisible = false;
        }
    }
}