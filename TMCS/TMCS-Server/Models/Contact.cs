using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCS.Server.Models
{
    public class Contact
    {
        public string SubjectId { get; set; }
        public string ObjectId { get; set; }
        public User Subject { get; set; }
        public User Object { get; set; }
        public string Note { get; set; }
    }
}
