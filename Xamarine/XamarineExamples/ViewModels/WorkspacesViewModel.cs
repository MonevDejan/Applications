using ProjectInsightMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectInsightMobile.ViewModels
{
   public class WorkspacesViewModel : BaseViewModel
    {
        public ObservableCollection<Workspace> Items { get; set; }

    }
}
