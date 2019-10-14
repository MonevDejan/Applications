using ProjectInsight.Models.Notifications;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
       // public Command LoadItemsCommand { get; set; }


        ObservableCollection<Notification> notifications;
        public ObservableCollection<Notification> Notifications
        {
            set
            {
                if (notifications != value)
                {
                    notifications = value;     
                    foreach (var item in notifications)
                    {
                        if (item.IconImageUrl == null)
                            item.IconImageUrl = "https://cdn0.iconfinder.com/data/icons/social-messaging-ui-color-shapes/128/notification-circle-blue-512.png";
                    }       
                    OnPropertyChanged("Notifications");
                }
            }
            get
            {
                return notifications;
            }
        }

       public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
            Title = "Notifications";                                                   
        }

        //async Task ExecuteLoadItemsCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        notification.Clear();
        //        //var result = await NotificationsService.Get();
        //        //notification = new ObservableCollection<ProjectInsight.Models.Notifications.Notification>(result);


        //        Notifications.Add(new Notification() {
        //            Body= "Test",
        //            CreatedDateTimeUTC = DateTime.Now,
        //            IconImageUrl = "issss"
        //        });
        //        Notifications.Add(new Notification()
        //        {
        //            Body = "Test",
        //            CreatedDateTimeUTC = DateTime.Now,
        //            IconImageUrl = "issss"
        //        });
        //        Notifications.Add(new Notification()
        //        {
        //            Body = "Test",
        //            CreatedDateTimeUTC = DateTime.Now,
        //            IconImageUrl = "issss"
        //        });
        //        notification = Notifications;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
    }


    public class Notification
    {
        public bool IsBusy { get; set; }
        public bool ShowUserAvatar { get; set; }
        public bool ShowUserPhoto { get; set; }
        public string AvatarInitials { get; set; }
        public string AvatarColor { get; set; }


        public Guid ObjectEvent_Id { get; set; }
        public Guid Object_Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDateTimeUTC { get; set; }
        public string IconUrl { get; set; }
        public string IconImageUrl { get; set; }
        public string UserPhotoImageUrl { get; set; }
        public string UserAvatarHTML { get; set; }
        public Guid UserCreated_Id { get; set; }
        public string ObjectTypeString { get; set; }

    }
    }
