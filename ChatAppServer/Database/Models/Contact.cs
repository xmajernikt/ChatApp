using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Models
{
    internal class Contact
    {
        private Guid userId { get; set; }
        private string Username {  get; set; }
        private string ImageSrc { get; set; }
        private string LastMessage { get; set; }
        private DateTime createdAt { get; set; }
    }
}
