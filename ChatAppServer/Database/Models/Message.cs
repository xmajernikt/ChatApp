using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Models
{
    internal class Message
    {
        private Guid messageId {  get; set; }
        private Guid userId {  get; set; }
        private Guid contactId {  get; set; }
        private string message {  get; set; }
        private bool IsNativeOriginated { get; set; }
        private bool IsFirstMessage {  get; set; }

    }
}
