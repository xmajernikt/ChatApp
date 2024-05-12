using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Models
{
    internal class User
    {
        public string? Username { get; set; }
        public string? ImageSrc { get; set; }
        public DateTime CreatedAt {  get; set; }  
    }
}
