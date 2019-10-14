using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class PushNotificationSettings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
