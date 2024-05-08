using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.MVVM.Model
{
    internal class MessageModel
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public string UsernameColor { get; set; }
        public string ImageSrc { get; set; }
        public bool IsNativeOrigined { get; set; }
        public bool IsFirstMessage { get; set; }
        public DateTime Time { get; set; }

    }
}
