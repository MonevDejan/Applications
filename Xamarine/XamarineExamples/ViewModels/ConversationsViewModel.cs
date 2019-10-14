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
    public class ConversationsViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<Conversation> Conversations { get; set; }

        public ConversationsViewModel()
        {
            Title = "Messages";

            Conversations = new ObservableCollection<Conversation>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

      

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                // mock data, get from api
                Conversations.Add(new Conversation() { WithName = "Coach Jay Smith", LastMessage = "Practice is moved to East", LastActivityRelative= "Tuesday", HasLike=false });
                Conversations.Add(new Conversation() { WithName = "Coach Jay Smith", LastMessage = "", LastActivityRelative= "Wednesday", HasLike = true });
                Conversations.Add(new Conversation() { WithName = "Coach Mike Jones", LastMessage = "No. I didn’t get the relay card.", LastActivityRelative= "Yesterday", HasLike = false });
                Conversations.Add(new Conversation() { WithName = "Coach Dave Durden", LastMessage = "", LastActivityRelative= "Yesterday", HasLike = true });
                Conversations.Add(new Conversation() { WithName = "Coach Bob Bowman", LastMessage = "", LastActivityRelative= "Yesterday", HasLike = true });
                Conversations.Add(new Conversation() { WithName = "Coach Jay Smith", LastMessage = "Practice is moved to East", LastActivityRelative= "6:30AM", HasLike = false });

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
