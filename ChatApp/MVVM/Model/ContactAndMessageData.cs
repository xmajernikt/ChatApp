using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.MVVM.Model
{
    public class ContactAndMessageData
    {
        public List<ContactModel> Contacts { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}
