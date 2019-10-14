
using Xamarin.Forms;

namespace ProjectInsightMobile.CustomControls
{
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty HasBorderProperty = BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomEditor), false);

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }
    }
}
