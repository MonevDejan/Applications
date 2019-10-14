using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ProjectInsightMobile.Models;
using ProjectInsightMobile.Views;
using ProjectInsight.Models.Users;
using System.Linq;
using ProjectInsightMobile.Helpers;
using System.Collections.Generic;
using ProjectInsightMobile.Services;
using ProjectInsight.Models.Files;
using ProjectInsight.Models.Folders;

namespace ProjectInsightMobile.ViewModels
{
    public class FileDetailsViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        string created { get; set; }
        public string Created
        {
            set
            {
                if (created != value)
                {
                    created = value;
                    OnPropertyChanged("Created");
                }
            }
            get
            {
                return created;
            }
        }
        string updated { get; set; }
        public string Updated
        {
            set
            {
                if (updated != value)
                {
                    updated = value;
                    OnPropertyChanged("Updated");
                }
            }
            get
            {
                return updated;
            }
        }

        string icon { get; set; }
        public string Icon
        {
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }
            get
            {
                return icon;
            }
        }

        Boolean canDelete { get; set; }
        public Boolean CanDelete
        {
            set
            {
                if (canDelete != value)
                {
                    canDelete = value;
                    OnPropertyChanged("CanDelete");
                }
            }
            get
            {
                return canDelete;
            }
        }


    }
}