
using Xamarin.Forms;

namespace ProjectInsightMobile.CustomControls
{
    public class CustomLabel : Label
    {
        public static readonly BindableProperty IsUnderlinedProperty = BindableProperty.Create("IsUnderlined", typeof(bool), typeof(CustomLabel), false);

        public bool IsUnderlined
        {
            get { return (bool)GetValue(IsUnderlinedProperty); }
            set { SetValue(IsUnderlinedProperty, value); }
        }
    }
}
