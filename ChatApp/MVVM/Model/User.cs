using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.MVVM.Model
{
    internal class User : EventArgs
    {
        public string Username { get; set; }
        public string ImageSrc { get; set; }
        public Guid UserId { get; set; }

    }
}
