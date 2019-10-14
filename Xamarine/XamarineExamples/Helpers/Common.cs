using Acr.UserDialogs;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsight.Models.Comments;
using ProjectInsightMobile.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Plugin.DeviceInfo;
using ProjectInsight.Models.Users;
using ProjectInsight.Models.Workspace;
using ProjectInsightMobile.CustomControls;

namespace ProjectInsightMobile.Helpers
{
    public class Common
    {
        private static Common instance;
        private Common()
        {

        }

        public static Common Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Common();
                }
                return instance;
            }
        }
        public BottomNavigationViewModel bottomNavigationViewModel { get; set; } = new BottomNavigationViewModel();
        public string DocumentFilePath { get; set; } = string.Empty;
        public string PicturesPath { get; set; } = string.Empty;
        //public User user { get; set; }

        public static Workspace CurrentWorkspace { get; set; }


        public static Filter Filter { get; set; }

        static bool refreshProjectsList;
        public static bool RefreshProjectsList
        {
            set
            {
                refreshProjectsList = value;
            }
            get
            {
                if (refreshProjectsList)
                {
                    refreshProjectsList = false;
                    return true;
                }
                else
                    return false;

                //return refreshProjectsList;
            }
        }

        static bool refreshWorkList;
        public static bool RefreshWorkList
        {
            set
            {
                refreshWorkList = value;
            }
            get
            {
                if (refreshWorkList)
                {
                    refreshWorkList = false;
                    return true;
                }
                else
                    return false;
            }
        }

        public static UserGlobalCapability UserGlobalCapability { get; set; }
        public static WorkspaceCapability WorkspaceCapability{ get; set; }

        public byte[] Base64ImageSource { get; set; } = null;

        static object locker = new object();

        public static PushNotificationContent PushNotificationAction = null;

        public static string WorkspaceApi
        {
            get
            {
                return Common.CurrentWorkspace.WorkspaceURL + "/api";
            }
        }

        #region DB       
        public SQLiteConnection _sqlconnection;
        public SQLiteConnection InitializeDatabase()
        {

            ////InitialzeDatabase for path     
            try
            {
                if (_sqlconnection == null)
                    _sqlconnection = DependencyService.Get<ISQLiteService>().GetConnection();
            }
            catch (Exception ex)
            {

            }
            //return _sqlconnection;
            return _sqlconnection;
        }

        public void CreateTable<T>()
        {
            try
            {
                lock (locker)
                {
                    _sqlconnection.CreateTable<T>();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public int DeleteAll<T>()
        {
            lock (locker)
            {
                try
                {
                    //if (_sqlconnection.TableMappings.Any(x => x.MappedType.FullName == typeof(T).FullName))
                    return _sqlconnection.DeleteAll<T>();
                    //else
                    //   return 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int DeleteByID<T>(int id)
        {
            lock (locker)
            {
                return _sqlconnection.Delete<T>(id);
            }
        }
        //Get generic class
        public IEnumerable<T> GetAll<T>() where T : new()
        {
            lock (locker)
            {
                return (from i in _sqlconnection.Table<T>() select i).ToList();
            }
        }

        // get list of MyWork
        public IEnumerable<MyWorkItem> GetUserWork()
        {
            try
            {
                return (from t in _sqlconnection.Table<MyWorkItem>() select t).ToList();
            }
            catch (Exception ex)
            {
                List<MyWorkItem> pom = new List<MyWorkItem>();
                return pom.ToList();
            }
        }

        public class TableName
        {
            public TableName() { }
            public string name { get; set; }
        }
        public void CheckLocalTables()
        {
            string queryString = $"SELECT name FROM sqlite_master WHERE type = 'table'";
            var allTables = _sqlconnection.Query<TableName>(queryString);

            foreach (var item in allTables)
            {
                string queryString1 = $"SELECT * FROM " + item.name;
                //var table = _sqlconnection.Query<Object>(queryString1);

                if (item.name == "AppVersion")
                {
                    var table1 = _sqlconnection.Query<dynamic>(queryString1);
                }
                else if (item.name == "sqlite_sequence")
                {
                    //List<sqlite_sequence> table1 = _sqlconnection.Query<sqlite_sequence>(queryString1);
                }
                else if (item.name == "PushNotificationSettings")
                {
                    List<PushNotificationSettings> table1 = _sqlconnection.Query<PushNotificationSettings>(queryString1);
                }
                else if (item.name == "Workspace")

                {
                    List<Workspace> table1 = _sqlconnection.Query<Workspace>(queryString1);
                }
                else if (item.name == "Settings")
                {
                    List<Settings> table1 = _sqlconnection.Query<Settings>(queryString1);
                }
                else if (item.name == "MyWorkItem")
                {
                    List<MyWorkItem> table1 = _sqlconnection.Query<MyWorkItem>(queryString1);
                }

            }
        }

        public List<Object> GetTable(string tableName)
        {
            string queryString = $"SELECT * FROM " + tableName;
            return _sqlconnection.Query<Object>(queryString);

        }
        public IEnumerable<Workspace> GetWorkspaces()
        {
            try
            {
                Common.Instance.CreateTable<Workspace>();
                return (from t in _sqlconnection.Table<Workspace>() select t).ToList();
            }
            catch (Exception ex)
            {
                List<Workspace> pom = new List<Workspace>();
                return pom.ToList();
            }
        }

        public IEnumerable<Settings> GetSettings()
        {
            try
            {
                Common.Instance.CreateTable<Settings>();
                return (from t in _sqlconnection.Table<Settings>() select t).ToList();
            }
            catch (Exception ex)
            {
                List<Settings> pom = new List<Settings>();
                return pom.ToList();
            }
        }

        public Boolean StoreToken(string AppToken)
        {
            SQLiteConnection connection = Common.Instance.InitializeDatabase();
            Common.Instance.CreateTable<PushNotificationSettings>();

            Instance.DeleteAll<PushNotificationSettings>();
            bool isPushEnabled;
            try
            {
                isPushEnabled = DependencyService.Get<INotificationsInterface>().registeredForNotifications();
            }
            catch (Exception ex)
            {
                isPushEnabled = true;
            }

            PushNotificationSettings setting = new PushNotificationSettings()
            {
                Token = AppToken,
                CreatedAt = DateTime.Now
            };
            try
            {
                Common.Instance._sqlconnection.Insert(setting);
            }
            catch
            {
                return false;
            }

            return true;
        }


        public Boolean SetAppVersion(string verNumber)
        {
            SQLiteConnection connection = Common.Instance.InitializeDatabase();



            if (!Common.TableExists<AppVersion>())
                Common.Instance.CreateTable<AppVersion>();

            IEnumerable<AppVersion> versions = Common.Instance.GetAll<AppVersion>();
            AppVersion version = versions.FirstOrDefault();
            if (version == null || version.Version != verNumber)
            {
                //Update vernumber and delete local storage tables
                //Instance._sqlconnection.DropTable<Workspace>();
                Instance._sqlconnection.DropTable<MyWorkItem>();


                Instance.DeleteAll<AppVersion>();
                AppVersion ver = new AppVersion()
                {
                    Version = verNumber,
                    CreatedAt = DateTime.Now
                };
                try
                {
                    Instance._sqlconnection.CreateTable<MyWorkItem>();
                    Common.Instance._sqlconnection.Insert(ver);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool TableExists<T>()
        {
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = Common.Instance._sqlconnection.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }
        public void SendPushNotificationToken()
        {
            try
            {


                string deviceId = CrossDeviceInfo.Current.Id;

                IEnumerable<PushNotificationSettings> settings = Common.Instance.GetAll<PushNotificationSettings>();
                PushNotificationSettings setting = settings.FirstOrDefault();
                if (setting != null)
                {
                    if (Common.CurrentWorkspace.PushNotifDateSent == null || setting.CreatedAt > Common.CurrentWorkspace.PushNotifDateSent)
                    {
                        PushNotificationService client = new PushNotificationService();
                        client.SetPushNotificationTokenLegacy(setting.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                //ignore if PushNotificationSettings doesn't exist
            }
        }

        #endregion

        #region Helpers Methods 
        public void ShowToastMessage(string msg, System.Drawing.Color typeColor)
        {
            var toastConfig = new ToastConfig(msg);
            toastConfig.SetDuration(DoubleResources.DURATION_TOAST);
            toastConfig.SetBackgroundColor(typeColor);
            toastConfig.Position = ToastPosition.Top;
            UserDialogs.Instance.Toast(toastConfig);
        }

        public bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                return false;
            else
            {
                var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            }
        }
        #endregion
    }
}
