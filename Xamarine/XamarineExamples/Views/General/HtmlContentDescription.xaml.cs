using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.General
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HtmlContentDescription : ContentPage
	{

        DescriptionViewModel viewModel;
        public HtmlContentDescription (DescriptionViewModel viewModel)
		{
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = this.viewModel = viewModel;
			InitializeComponent ();

            if(Common.Instance.Base64ImageSource != null)
            {

                Stream stream = new MemoryStream(Common.Instance.Base64ImageSource);
                var imageSource = ImageSource.FromStream(() => stream);
                imgHtml.Source = imageSource;

            }
        }
	}
}