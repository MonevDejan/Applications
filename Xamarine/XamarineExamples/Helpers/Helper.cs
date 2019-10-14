using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjectInsightMobile.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetHexString(this Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

            return hex;
        }

        public static string ConvertColorToHex(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
            return hex;
        }
    }
}
