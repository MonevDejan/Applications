using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SafeSportChat.Models;

namespace SafeSportChat.ViewModels
{
    public class MessagesViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public Command LoadItemsCommand { get; set; }

        public MessagesViewModel()
        {
            Title = "Coach Jay Smith";

            Messages = new ObservableCollection<Message>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }



        //   private ObservableCollection<Message> messagesList;

        public ObservableCollection<Message> Messages { get; set; }






        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                // mock data, get from api


                Messages.Add(new Message { Text = "When are we practicing next? I forgot my calendar.", IsIncoming = false, MessagDateTime = DateTime.Now.AddMinutes(-25) });
                Messages.Add(new Message { Text = "The team is practicing tomorrow morning. We will not be on Saturday.", IsIncoming = true, MessagDateTime = DateTime.Now.AddMinutes(-24) });
                Messages.Add(new Message { Text = "Thanks. Can you tell me what I need for the meet?", IsIncoming = false, MessagDateTime = DateTime.Now.AddMinutes(-23) });
                Messages.Add(new Message { Text = "Have your bag and parka ready to put on the bus when you arrive. We are all excited about the meet and are glad you could come along.", IsIncoming = true, MessagDateTime = DateTime.Now.AddMinutes(-22) });



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }

}
