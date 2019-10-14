using ProjectInsight.Models.Comments;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectInsightMobile.ViewModels
{
    public class DocumentViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ItemType ItemType { get; set; }
        public Boolean IsFolder { get; set; }
    }

    
}