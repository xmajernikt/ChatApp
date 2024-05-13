using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.MVVM.Model
{
    public class ContactModel
    {
        Guid Id { get; set; }
        public string Username { get; set; }
        public string ImageSrc { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public string LastMessage {  get; set; }
        //public string LastMessage => Messages.Last().Message;


    }
}
