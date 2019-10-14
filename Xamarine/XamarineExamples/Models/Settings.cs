using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
