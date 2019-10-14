
using Xamarin.Forms;

namespace ProjectInsightMobile.CustomControls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty HasBorderProperty = BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomEntry), false);

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }
    }
}
