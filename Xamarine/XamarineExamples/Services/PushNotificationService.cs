using Newtonsoft.Json;
using ProjectInsight.Models.Base.SimpleTypes;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace ProjectInsightMobile.Services
{
   
public class PushNotificationService : ProjectInsight.Models.Base.RESTClientBase
    {
        public PushNotificationService()
        {
            this.RESTBaseUri = Common.WorkspaceApi;

            this.AuthenticationToken = Common.CurrentWorkspace.ApiToken;

            //adds the default 
            this.AddDefaultRequestHeadersToClient();
        }
        public PushNotificationService(Workspace ws)
        {
            this.RESTBaseUri = ws.WorkspaceURL + "/api";

            this.AuthenticationToken = ws.ApiToken;

            //adds the default 
            this.AddDefaultRequestHeadersToClient();
        }

        public override void AddDefaultRequestHeadersToClient()
        {
            this.Client.DefaultRequestHeaders.Clear();

            if (!String.IsNullOrEmpty(this.AuthenticationToken))
            {
                //this.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AuthenticationToken);
                this.Client.DefaultRequestHeaders.Add("Api-Token", this.AuthenticationToken);
            }
        }

        /// <summary>
        /// Sends the <see cref="String"/> object through the firebase cloud messaging server
        /// </summary>
        /// <param name="token"><see cref="string"/> to send</param>
        /// <returns><see cref="bool"/> response from the firebase cloud messaging server</returns>
        public async  void SetPushNotificationTokenLegacy(String sToken)
        {
            try
            {
            
                SimpleString token = new SimpleString();
                token.Value = sToken;
                HttpResponseMessage resp = await this.PostAsync(Common.WorkspaceApi +  "/user/set-push-notification-token", token);
                //HttpResponseMessage resp = this.Post("https://gjokokoco-demo.projectinsight.net/api/user/set-push-notification-token", sToken);

                Workspace ws = Common.CurrentWorkspace;
                if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(sToken))
                {
                    ws.PushNotifDateSent = DateTime.Now;
                }
                else
                {
                    ws.PushNotifDateSent = null;
                }
                Common.Instance._sqlconnection.Update(ws);
            }
            catch (Exception ex)
            {
                Workspace ws = Common.CurrentWorkspace;
                ws.PushNotifDateSent = null;
                Common.Instance._sqlconnection.Update(ws);
            }
        }


        /// <summary>
        /// Sets the push the notification token  for the supplied device id, token and <see cref="User"/>.  URL: /api/set-push-notification
        /// </summary>
        /// <param name="deviceId"><see cref="SimpleString"/> device ID (id of the phone)</param>
        /// <param name="pushNotificationToken"> <see cref="SimpleString"/> push notification token</param>	
        /// <returns><see cref="User"/> matching the Id.</returns>


        public HttpResponseMessage SetPushNotificationToken(String DeviceId, String Token)
        {
            try
            {
                SimpleString pushNotificationToken = new SimpleString();
                pushNotificationToken.Value = Token;

                this.AddQueryParam("deviceId", DeviceId.ToString());

                HttpResponseMessage resp = this.Post(this.RESTBaseUri + "/user/set-push-notification", pushNotificationToken);

                return resp;
               
            }
            catch (Exception ex)
            {
                return null;
                //Workspace ws = Common.CurrentWorkspace;
                //ws.PushNotifDateSent = null;
                //Common.Instance._sqlconnection.Update(ws);
            }
        }
        public async void RemovePushNotificationTokenForCurrentUser(String DeviceId)
        {
            try
            {
                SimpleString ssDeviceId = new SimpleString();
                ssDeviceId.Value = DeviceId;

                HttpResponseMessage resp = await this.PostAsync(Common.WorkspaceApi + "/user/remove-push-notification", ssDeviceId);

                if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(DeviceId))
                {
                    Workspace currentWS = Common.CurrentWorkspace;
                    currentWS.PushNotifDateSent = null;

                    var workspaces = Common.Instance.GetWorkspaces();
                    foreach (Workspace ws in workspaces)
                    {
                        if (ws.Id == currentWS.Id)
                        {
                            ws.PushNotifDateSent = null;
                            Common.Instance._sqlconnection.Update(ws);
                            break;
                        }
                    }

                }
                //Common.Instance.DeleteAll<PushNotificationSettings>();

            }
            catch (Exception ex)
            {

            }
        }

        public async void RemovePushNotificationTokenForWorkspace(String DeviceId)
        {
            try
            {
                SimpleString ssDeviceId = new SimpleString();
                ssDeviceId.Value = DeviceId;

                HttpResponseMessage resp = await this.PostAsync(this.RESTBaseUri + "/user/remove-push-notification", ssDeviceId);

                if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(DeviceId))
                {

                }
                //Common.Instance.DeleteAll<PushNotificationSettings>();

            }
            catch (Exception ex)
            {

            }
        }
        public async void RemovePushNotificationTokenForAllWorkspaces(String DeviceId)
        {
            try
            {
                SimpleString ssDeviceId = new SimpleString();
                ssDeviceId.Value = DeviceId;

                HttpResponseMessage resp = await this.PostAsync(Common.WorkspaceApi + "/user/remove-push-notification-all", ssDeviceId);

                if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(DeviceId))
                {
                    Workspace currentWS = Common.CurrentWorkspace;
                    currentWS.PushNotifDateSent = null;

                    var workspaces = Common.Instance.GetWorkspaces();
                    foreach (Workspace ws in workspaces)
                    {
                        ws.PushNotifDateSent = null;
                        Common.Instance._sqlconnection.Update(ws);
                    }

                }
                //Common.Instance.DeleteAll<PushNotificationSettings>();

            }
            catch (Exception ex)
            {

            }
        }

        
    }
}
