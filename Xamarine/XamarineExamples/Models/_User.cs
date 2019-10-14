using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class User_OLD 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateTimeCreated { get; set; }
        public Guid UserID { get; set; }
        public string UserToken { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int WorkspaceId { get; set; }

    }
}
