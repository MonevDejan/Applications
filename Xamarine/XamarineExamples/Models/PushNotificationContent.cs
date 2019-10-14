using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class PushNotificationContent
    {
        public Guid ObjectId { get; set; }
        public string ObjectTypeString { get; set; }
        public Guid DomainId { get; set; }
    }
}
