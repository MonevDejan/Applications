using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjectInsightMobile.Helpers
{
    public class DoubleResources
    {
        public static readonly Thickness ButtonGroupPadding = new Thickness(0, Device.OnPlatform(25, 0, 0), 0, 0);
        public static readonly double SignUpButtonHeight = Device.OnPlatform(40, 40, 80);
        public static readonly double FBButtonHeight = Device.OnPlatform(50, 50, 64);
        public static readonly System.Drawing.Color SuccessSnackBar = System.Drawing.Color.FromArgb(76, 175, 80);
        public static readonly System.Drawing.Color DangerSnackBar = System.Drawing.Color.FromArgb(253, 56, 56);
        public static readonly int DURATION_TOAST = 2000;
    }
}
