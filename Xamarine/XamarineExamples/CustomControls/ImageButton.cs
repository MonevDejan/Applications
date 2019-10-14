using System;
using Xamarin.Forms;

namespace ProjectInsightMobile.CustomControls
{
    class ImageButton : Image
    {
        public event EventHandler Clicked;
        protected void OnClick(EventArgs e)
        {
            if (Clicked != null)
                Clicked(this, e);
        }

        public ImageButton()
        {
            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;
            this.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    OnClick(null);
                })
            });
        }
    }
}
