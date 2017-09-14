using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TMCS.Server.Models
{
    public class Token
    {
        [Key]
        public string Id { get; set; }
        public User User { get; set; }
        public DateTime Expire { get; set; }
    }
}
