using Plugin.DeviceInfo;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace ProjectInsightMobile.Helpers
{
    public static class PushNotifications
    {

        public static void ConfigurePushNotificationSettings()
        {
            try
            {
               SettingsService settings = new SettingsService();

                bool isPushEnabled;
                try
                {
                    isPushEnabled = DependencyService.Get<INotificationsInterface>().registeredForNotifications();
                }
                catch (Exception ex)
                {
                    isPushEnabled = true;
                }

                bool updateAllWorkspaces = false;
                ///9999 is general setting  for all workspaces
                var pn_enabled = settings.Get("PushNotifications_isEnabled",9999);
                if (isPushEnabled != (pn_enabled == "1"))
                {
                    updateAllWorkspaces = true;
                    settings.Set("PushNotifications_isEnabled", (isPushEnabled ? "1" : "0"),9999);
                }
                

                //bool hasActivatedOrDeactivated = false;

                
                Common.Instance.CreateTable<PushNotificationSettings>();

                IEnumerable<PushNotificationSettings> pn_settings = Common.Instance.GetAll<PushNotificationSettings>();
                PushNotificationSettings setting = pn_settings.FirstOrDefault();

                //if (setting != null && setting.PN_Enabled != isPushEnabled)
                //{
                //    hasActivatedOrDeactivated = true;
                //    setting.PN_Enabled = isPushEnabled;
                //    setting.CreatedAt = DateTime.Now;
                //    Common.Instance._sqlconnection.Update(setting);
                //}

                string deviceId = CrossDeviceInfo.Current.Id;

                if (isPushEnabled)
                {
                    //send FCN token for all workspaces where the user is logged in
                    //User has enabled the PN, send token for all logged in workspaces
                    var workspaces = Common.Instance.GetWorkspaces();
                    foreach (Workspace ws in workspaces)
                    {
                        if (!string.IsNullOrEmpty(ws.ApiToken))
                        {
                            //if (Common.CurrentWorkspace.PushNotifDateSent == null || (setting != null && setting.CreatedAt > Common.CurrentWorkspace.PushNotifDateSent) || updateAllWorkspaces)
                            if (ws.PushNotifDateSent == null || (setting != null && setting.CreatedAt > ws.PushNotifDateSent) || updateAllWorkspaces)

                            {
                                PushNotificationService client = new PushNotificationService();
                                client.RESTBaseUri = ws.WorkspaceURL + "/api";
                                client.AuthenticationToken = ws.ApiToken;
                                HttpResponseMessage resp = client.SetPushNotificationToken(deviceId, setting.Token);

                                DateTime now = new DateTime();
                                now = DateTime.Now;

                                if (resp != null && resp.IsSuccessStatusCode)
                                    ws.PushNotifDateSent = now;
                                else
                                    ws.PushNotifDateSent = null;
                                Common.Instance._sqlconnection.Update(ws);

                                if (ws.Id == Common.CurrentWorkspace.Id)
                                    Common.CurrentWorkspace.PushNotifDateSent = now;
                            }
                        }
                    }
                }
                else
                {
                    if (updateAllWorkspaces)
                    {
                        //User has disabled the PN, remove PN token for all workspaces
                        //delete token on server
                        PushNotificationService client = new PushNotificationService();
                        client.RemovePushNotificationTokenForAllWorkspaces(deviceId);
                    }
                }


                //if (updateAllWorkspaces)
                //{
                //    //the user has activated or deactivated PN
                   

                //}
                //else
                //{
                //    //no change on Enabled/disabled on receiving PN 

                //    if (setting != null)
                //    {
                //        if (Common.CurrentWorkspace.PushNotifDateSent == null || setting.CreatedAt > Common.CurrentWorkspace.PushNotifDateSent)
                //        {
                //            PushNotificationService client = new PushNotificationService();
                //            ////client.SetPushNotificationTokenLegacy(setting.Token);

                //            HttpResponseMessage resp = client.SetPushNotificationToken(deviceId, setting.Token);
                //            DateTime now = new DateTime();
                //            now = DateTime.Now;
                //            Workspace ws = Common.CurrentWorkspace;
                //            if (resp.IsSuccessStatusCode)
                //                ws.PushNotifDateSent = now;
                //            else
                //                ws.PushNotifDateSent = null;
                //            Common.Instance._sqlconnection.Update(ws);
                //        }
                //    }
                //}
              
            }
            catch (Exception ex)
            {
                //ignore if PushNotificationSettings doesn't exist
            }
        }


    }
}
