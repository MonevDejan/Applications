//using SQLite;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class Filter
    {
        //[PrimaryKey, AutoIncrement]

       
        public int Id { get; set; }

        Guid? companyId { get; set; }
        public Guid? CompanyId
        {
            set
            {
                Common.RefreshWorkList = true;
                Common.RefreshProjectsList = true;
                companyId = value;
            }
            get
            {
                return companyId;
            }
        }

    }
}
