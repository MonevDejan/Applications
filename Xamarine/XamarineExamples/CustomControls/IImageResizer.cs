using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.CustomControls
{
    public interface IImageResizer
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
       
}
