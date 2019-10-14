using System;
using System.Collections.Generic;
using System.Text;
using ProjectInsight.WebApi.Client.Notifications;
using ProjectInsight.Models.Notifications;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{
    public class NotificationsService
    {
       public static NotificationClient client;

        public static async Task<List<Notification>> Get(DateTime? beforeDateTimeUTC = null, int? count = null)
        {

           
            try
            {
                List<Notification> notifications = await client.ListByNotificationCommentsAsync(beforeDateTimeUTC, count);
                return notifications;
            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return null;
            }
        }


    }
}
