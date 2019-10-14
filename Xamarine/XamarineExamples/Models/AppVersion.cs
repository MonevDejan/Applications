using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class AppVersion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
