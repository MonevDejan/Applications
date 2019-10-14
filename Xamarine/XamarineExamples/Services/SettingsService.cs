using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsight.WebApi.Client.Users;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class SettingsService
    {
        public SQLiteConnection connection;
        public  SettingsService() {
            connection = Common.Instance.InitializeDatabase();
            Common.Instance.CreateTable<Settings>();
        }
        public bool Set(string name, string value)
        {
            try
            {
                var settings = Common.Instance.GetSettings();

                Settings setting = settings.Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.Name== name).FirstOrDefault();
                if (setting != null)
                {
                    setting.Value = value;
                    Common.Instance._sqlconnection.Update(setting);
                }
                else
                {
                    setting = new Settings(){WorkspaceId = Common.CurrentWorkspace.Id, Name = name, Value = value};
                    Common.Instance._sqlconnection.Insert(setting);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

        }

        public string Get(string name)
        {
            try
            {
                var settings = Common.Instance.GetSettings();

                Settings setting = settings.Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.Name == name).FirstOrDefault();
                if (setting != null)
                    return setting.Value;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;

            }

        }

        public string Get(string name, int workspaceId)
        {
            try
            {
                var settings = Common.Instance.GetSettings();

                Settings setting = settings.Where(x => x.WorkspaceId == workspaceId && x.Name == name).FirstOrDefault();
                if (setting != null)
                    return setting.Value;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;

            }

        }
        public bool Set(string name, string value, int workspaceId)
        {
            try
            {
                var settings = Common.Instance.GetSettings();

                Settings setting = settings.Where(x => x.WorkspaceId == workspaceId && x.Name == name).FirstOrDefault();
                if (setting != null)
                {
                    setting.Value = value;
                    Common.Instance._sqlconnection.Update(setting);
                }
                else
                {
                    setting = new Settings() { WorkspaceId = workspaceId, Name = name, Value = value };
                    Common.Instance._sqlconnection.Insert(setting);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

        }


    }

}
