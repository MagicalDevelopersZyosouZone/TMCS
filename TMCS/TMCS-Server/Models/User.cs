using System;
using System.Collections.Generic;

namespace TMCS.Server.Models
{
    public class User
    {
        public string Id { get; set; }
        public byte[] PublicKey { get; set; }
        public string NickName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public byte[] PrivateKeyEncrypted { get; set; }
        public List<User> Friends { get; set; }
    }
}
