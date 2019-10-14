using ProjectInsight.Models.SysAdmin;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Models
{
    public class Workspace
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public Guid DomainID { get; set; }
        public string Name { get; set; }
        public string WorkspaceURL { get; set; }
        public bool SSOEnabled { get; set; }
        public bool EmailPasswordEnabled { get; set; }
        public bool isActive { get; set; }
        public string ApiToken { get; set; }
        public Guid UserID { get; set; }
        public DateTime? PushNotifDateSent { get; set; }


        [Ignore]
        public string Icon { get; set; }

        public static implicit operator Workspace(ProjectInsight.Models.SysAdmin.Workspace v)
        {
            throw new NotImplementedException();
        }
    }
}
