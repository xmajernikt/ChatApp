﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Models
{
    public class Message
    {
        public Guid Id {  get; set; }
        public string UsernameID {  get; set; }
        public Guid ContactId {  get; set; }
        public string? MessageText {  get; set; }
        public bool IsNativeOriginated { get; set; }
        public bool IsFirstMessage {  get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
