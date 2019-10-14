using SafeSportChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeSportChat
{
    class MyDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        public DataTemplate IncomingDataTemplate { get; set; }
        public DataTemplate OutgoingDataTemplate { get; set; }

        public MyDataTemplateSelector()
        {
            // Retain instances!
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));

            
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //    var messageVm = item as Message;
            //    if (messageVm == null)
            //        return null;

            //    return messageVm.IsIncoming ? this.IncomingDataTemplate : this.OutgoingDataTemplate;

            var messageVm = item as Message;
            if (messageVm == null)
                return null;
            return messageVm.IsIncoming ? this.incomingDataTemplate : this.outgoingDataTemplate;
        }

        private readonly DataTemplate incomingDataTemplate;
        private readonly DataTemplate outgoingDataTemplate;
    }
}
