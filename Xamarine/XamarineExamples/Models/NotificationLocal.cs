using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class NotificationLocalOld
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public Guid ObjectEvent_Id { get; set; }
        public Guid Object_Id { get; set; }

        public string Body { get; set; }
        public DateTime CreatedDateTimeUTC { get; set; }
        public string IconUrl { get; set; }
        public string IconImageUrl { get; set; }
        public Guid UserCreated_Id { get; set; }
        public string ObjectTypeString { get; set; }
    }
}
