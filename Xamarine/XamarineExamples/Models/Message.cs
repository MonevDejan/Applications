using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SafeSportChat.Models
{
    public class Message : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string From { get; set; }
        public int FromId { get; set; }

        public string Attachment { get; set; }
        public bool HasLike { get; set; }


        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; RaisePropertyChanged(); }
        }

        private DateTime messageDateTime;

        public DateTime MessagDateTime
        {
            get { return messageDateTime; }
            set { messageDateTime = value; RaisePropertyChanged(); }
        }

        private bool isIncoming;

        public bool IsIncoming
        {
            get { return isIncoming; }
            set { isIncoming = value; RaisePropertyChanged(); }
        }

        private void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;




    }
}
