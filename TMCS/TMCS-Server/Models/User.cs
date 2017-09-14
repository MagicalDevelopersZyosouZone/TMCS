using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TMCS.Server.Models
{
    public class User
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public byte[] PublicKey { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public byte[] PrivateKeyEncrypted { get; set; }
        public bool BlockStranger { get; set; }
        public AddContactPermission AddContactPermission { get; set; }

        public List<Contact> Contacts { get; set; }
    }

    public enum AddContactPermission
    {
        Allow,
        SendRequest,
        Deny
    }
}
