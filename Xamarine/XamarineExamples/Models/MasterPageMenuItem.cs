using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class MasterPageMenuItem
    {

        public int Id { get; set; }
        public string IconSource { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
