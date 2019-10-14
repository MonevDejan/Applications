using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProjectInsight.Models.Integrations.Google.FirebaseCloudMessaging;

namespace ProjectInsight.Apps.Integrations.Google
{
    /// <summary>
    /// Firebase Cloud Messaging Client used to handle push notifications
    /// </summary>
    public class FirebaseCloudMessagingRESTClient : ProjectInsight.Models.Base.RESTClientBase
    {

        /// <summary>
        /// Gets the <see cref="string"/> sender ID
        /// </summary>
        public string SenderId
        {
            get
            {
                return (string)System.Configuration.ConfigurationManager.AppSettings["Google.FirebaseSenderId"];
            }
        }

        /// <summary>
        /// Gets the <see cref="string"/> sender ID
        /// </summary>
        public string ServerKey
        {
            get
            {
                return (string)System.Configuration.ConfigurationManager.AppSettings["Google.FirebaseServerKey"];
            }
        }


        /// <summary>
        /// Default constructor for the cloud 
        /// </summary>
        public FirebaseCloudMessagingRESTClient()
        {

            this.RESTBaseUri = "https://fcm.googleapis.com/fcm/";

            //adds the default 
            this.AddDefaultRequestHeadersToClient();

        }



        /// <summary>
        /// Adds the default rest headers to the client
        /// </summary>
        public override void AddDefaultRequestHeadersToClient()
        {


            if (!String.IsNullOrEmpty(this.ServerKey))
            {
                this.Client.DefaultRequestHeaders.Add("Authorization", "Authorization: key=" + this.ServerKey);
            }

            if (!String.IsNullOrEmpty(this.SenderId))
            {
                this.Client.DefaultRequestHeaders.Add("Authorization", "Sender: id=" + this.SenderId);
            }
        }





        /// <summary>
        /// Sends the <see cref="FirebaseCloudMessagingNotification"/> object through the firebase cloud messaging server
        /// </summary>
        /// <param name="notification"><see cref="FirebaseCloudMessagingNotification"/> to send</param>
        /// <returns><see cref="FirebaseCloudMessagingResponse"/> response from the firebase cloud messaging server</returns>
        public FirebaseCloudMessagingResponse Send(FirebaseCloudMessagingNotification notification)
        {

            FirebaseCloudMessagingResponse fcmResp = null;

            HttpResponseMessage resp = this.Post("send", notification);

            if (resp.IsSuccessStatusCode)
            {
                fcmResp = Newtonsoft.Json.JsonConvert.DeserializeObject<FirebaseCloudMessagingResponse>(resp.Content.ReadAsStringAsync().Result);
            }

            return fcmResp;
        }


    }
}
