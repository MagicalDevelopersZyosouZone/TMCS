using System;
using System.Collections.Generic;

namespace TMCS.Server.Models
{
    public class Conversation
    {
        public User UserId { get; set; }
        public User Opposite { get; set; }
        //public Group Group { get; set; }
        public DateTime LastReadTime { get; set; }
    }
}
