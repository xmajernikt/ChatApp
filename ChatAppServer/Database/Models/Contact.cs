using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string UsernameId { get; set; }
        public string Username {  get; set; }
        public string ImageSrc { get; set; }
        public string LastMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
