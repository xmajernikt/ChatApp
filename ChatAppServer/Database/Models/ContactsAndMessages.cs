using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Models
{
    public class ContactAndMessageData
    {
        public List<Contact> Contacts { get; set; }
        public List<Message> Messages { get; set; }
    }
}
