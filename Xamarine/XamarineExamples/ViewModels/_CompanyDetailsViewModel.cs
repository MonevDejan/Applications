using ProjectInsight.Models.Comments;
using ProjectInsight.Models.Users;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectInsightMobile.ViewModels
{
    public class CompanyDetailsViewModel : BaseViewModel
    {
        public CompanyDetailsViewModel()
        {
            this.Id = string.Empty;
        }

        public string Id { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; }
        public bool? IsAutomaticallySetupCommunicateAddedCompany { get; set; }
        public string Address1 { get; set; }
        public bool? IsTimeAndExpenseEntryEnabled { get; set; }
        public bool? IsProjectAssignmentEnabled { get; set; }
        public bool? IsPermissionAssignmentEnabled { get; set; }
        public bool? IsActive { get; set; }
        public Guid? CustomFieldValue_Id { get; set; }
        public DateTime? CreatedDateTimeUTC { get; set; }
        public bool? IsInternal { get; set; }
        public string Name { get; set; }
        public string AllDetails{ get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}