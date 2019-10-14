using ProjectInsightMobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProjectInsightMobile.CustomControls
{
    public interface IPicturePicker
    {
        Task<ImageResult> GetImageStreamAsync();
    }
}


