using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Converters;

namespace TMCS_Test
{
    public enum UserStatus
    {
        Online,
        Offline
    }
    public class TMCSTest
    {



        public const string PUBLIC_KEY =
            @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCOE0MUSc/NWyHdX+VL27GSuZJo
fXMf9KKapokQhBIW0+UMMabim6TwKcmCZGiHonibvBKYKGluxdzhC39A6hndqAlY
G351ockJ6VrQd/JECV1vcFkdlnHVInBUKh/iQ1T8yPhZ4bSxFI9f+6Stlg2UCrat
wWm4Uhm43ulr13825wIDAQAB
-----END PUBLIC KEY-----";
        public const string PRIVATE_KEY =
            @"-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQCOE0MUSc/NWyHdX+VL27GSuZJofXMf9KKapokQhBIW0+UMMabi
m6TwKcmCZGiHonibvBKYKGluxdzhC39A6hndqAlYG351ockJ6VrQd/JECV1vcFkd
lnHVInBUKh/iQ1T8yPhZ4bSxFI9f+6Stlg2UCratwWm4Uhm43ulr13825wIDAQAB
AoGAIG7zREFpZ9bjDzdiEAQyMG2ep63jxyrAqA9WgOe1fXKX+kWndFnVuGHBt7uW
RUhowhyYHhYzo28V8mX+geIvTxph4NFVVBI+BVqu0bOW2FRGhAdGhx2uq2Qk5BQN
eLSV/va0fWCXxCAD5eIKOQ7o7aZQBqncbf3MEYEc/VNkEskCQQDuApprLJi2gYqY
XGKf6cHYFRv9PLedPzR79ongGTyLJW4asYgSpb9etlAx6MkVACvATbdCzlbZa1SK
EIwC0LzLAkEAmNBbuUiU8JUI7erUib2HZGnOq7AhHVK1rt9bs2MWAHxKniIQOpl7
Hd3AepfSErhJc8Xx/wqsmkT3BbMaPeQm1QJAPHE2y87IfViKXMThWuDfFEPQFtBL
hMhfSLfELb/a6y83NdotxyaGYcQEu5f1MLsZYT2bM4D49z/VeTZYiAKAXwJASIHU
g72kreKJMhpSbmi3bTWnEl+n1rn/6tGgOSWysthGr3GKMcPRXwJw6bpPuwImGAC8
Kj9uVmSSsOmicetnYQJAYtfo5ivoC3yySs5P+iOmlYSyzya/QBRvLNAFKeccY1rm
Cylj7GwF/uieA6xJbb4lZHMz0SHjxQkk1tt9ikT+fg==
-----END RSA PRIVATE KEY-----";

        public const string ENCRYPTED_PRIVATE_KEY = @"U2FsdGVkX19I2vEjMTt9CJxPJXfUb/r+AnIrvrTzijCKia4L33umC+wsc/TOLw8ELsjZcQGNVPqzHI7e/qCyajVywb6rncQc3VXFEZ8h4rdqQBqTlio0ZsLxYaYXpESNVasJfX2cHLnbqq3qCMm8lpCoFpPg8EFEDKK7RkWM3Bp3UEcN/dMMWMPSIfiZwOAVPdQCRYoVLmWTXox98TAMTR5tdLbbcuc2IEi6cvTwZ8MtAw5y+TXiEO2MEEY+NkKQWBF22j7RQNwLOTXA2DJTjg6JLQgs0cSnropTPJL6XmirVTVFpj2rJugzhHsItpgXMyw9jl4YSSXauHMZXerOUvASzMxBcQRn+BRE4S6lb8IMC9B3kJJVMcuMM75Evv5R8W+LwJFykMF1tq9fa2pMQ8JWxabDUIlc1cBn6B7JpqaA4Phd0sttdVUWBsjD7aDsJRKiWdD39cE9ekYqSO3WIgOZU4HW7TdbGdXzM/KwARbY5+eqLbOPQjGwHzkes3zHjb3LWHkEpe07rQbWHLu//iWCgswA/c7773m6kLSG4r0gN2GsTm0DUlEP8RekBDDbNJbP4idLFJlQbg/NSeps4+Gs4ZMwBZt86Q8+Kx2AKtD7xWjbwdXP44iEPzGRbpvPQss6gPr2sQW6DWkrlaDYcS8BoUcnQMr1YmFRw7D3hP8lau5+JWpqZjtAuWHwya6zegWFKQOy9EnSdYfXpWFDtb6OVLIjBquENKzxo18777ahYS8Z56Jhz6kywq6TgNUfx1986F+XGfBGrxbeHquVvf8wmSzgEALZoi1hv5Zn09RRXyUewbHtetX/DYLq5GG0lW8i8W8gZ6kg63rrct91dhyEV9/88PDHDN/nWC2vDQHjVDC+IvDBTUWcWZkw3sNXC/lpqCH/jxxkmOhPyDJlv1v5yGfuFLPXeo2U+QDqmN9l7i1BiqZ7QUrQqWu3a0VKGlRYztAcfXtaWjVVQ6j6QaMHxg/PyeEK1knyRk/9MdvlBqOutE850UPp5z9YeYzI9ZIdtfHOC7CLTGSdxorwh9nlunBWCvqspHZjxdthP3w9SUvqznGZhrDKH/MlAlpci99L+atWi660WYge9pVP0vu/f2SM9sf9yYTVaBiKfx2Nz7iR340N7DXI2al40cWypiIfSnqXukweOZZeOlBrG8rLJfI73c7gqpzxHFHzcuI=";
        public const string SALT = "Salt? Salt!";
        public const string AUTH_CODE = "I'm authCode";

        public static List<WebSocketHandler> HandlerList = new List<WebSocketHandler>();
        public static Random rand = new Random();
        public static string[] UserList = new string[] { "jack", "cherry", "BROWN", "Dwscdv3", "SardineFish" };

        [JsonObject]
        public class Message
        {
            [JsonProperty("senderId")]
            public string SenderId { get; set; }
            [JsonProperty("receiverId")]
            public string ReceiverId { get; set; }

            [JsonIgnore]
            public MessageData Data { get; set; }

            [JsonProperty("data")]
            public string EncryptedData
            {
                get
                {
                    var encTask = JSONStringifyAsync(this.Data);
                    encTask.Wait();
                    return TMCSTest.RSABlockEncrypt(Encoding.UTF8.GetBytes(encTask.Result), PUBLIC_KEY);
                }
            }

            [JsonProperty("type")]
            public string Type="Message";

            [JsonProperty("timeStamp")]
            public long TimeStamp { get; set; }

            [JsonObject]
            public class MessageData
            {
                [JsonProperty("type")]
                [JsonConverter(typeof(StringEnumConverter))]
                public MessageType Type { get; set; }

                [JsonProperty("data")]
                public string Data
                {
                    get;set;
                }
            }
            public Message(string senderId, string receiverId, MessageType msgType,string data)
            {
                this.SenderId = senderId;
                this.ReceiverId = receiverId;
                this.Data = new MessageData();
                this.Data.Data = data;
                this.Data.Type = msgType;
                this.TimeStamp = GetTimeStampNow();
            }

            public async Task<string> ToJSONAsync()
            {
                return await JSONStringifyAsync(this);
            }
        }
        public enum MessageType
        {
            Text,
            Image,
            Video,
            File
        }
        
        public static string RandomSalt(long length)
        {
            var salt = "";
            for(var i=0;i<length;i++)
            {
                salt += rand.Next(16).ToString("X");
            }
            return salt;
        }

        public static void CORS(HttpRequest request, HttpResponse response)
        {

            if (request.Headers.Keys.Contains("Origin"))
                response.Headers["Access-Control-Allow-Origin"] = request.Headers["Origin"];
            if (request.Headers.Keys.Contains("Access-Control-Request-Headers"))
                response.Headers["Access-Control-Allow-Headers"] = request.Headers["Access-Control-Request-Headers"];
            response.Headers["Access-Control-Allow-Credentials"] = "true";
        }

        public static object GetUserProfile(string uid)
        {
            if (uid == "Dwscdv3")
            {
                return new
                {
                    uid = uid,
                    nickName = "Dwscdv3",
                    status = "Online",
                    avatar = "http://img.sardinefish.com/NDgxMjA3",
                    pubKey = PUBLIC_KEY
                };
            }
            else if (uid == "SardineFish")
            {
                return new
                {
                    uid = uid,
                    nickName = "SardineFish",
                    status = "Online",
                    avatar = "http://img.sardinefish.com/NDgwOTM5",
                    pubKey = PUBLIC_KEY

                };
            }
            if (TMCSTest.rand.NextDouble() < 0.5)
            {
                return new
                {
                    uid = uid,
                    nickName = uid.ToUpper(),
                    sex = "Male",
                    status = "Online",
                    avatar = "http://img.sardinefish.com/NDc2NTU2",
                    pubKey = PUBLIC_KEY

                };
            }
            else
            {
                return new
                {
                    uid = uid,
                    nickName = uid.ToUpper(),
                    sex = "Female",
                    status = "Offline",
                    avatar = "http://img.sardinefish.com/NDc2NTU2",
                    pubKey = PUBLIC_KEY
                };
                
            }
        }

        public static string RandomUid()
        {
            return UserList[rand.Next(UserList.Length)];
        }

        public static byte[] ParseRSAKey(string key)
        {
            if (key.Contains("PUBLIC KEY") || key.Contains("PRIVATE KEY"))
            {
                var lines = key.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (lines.Count < 3)
                    throw new Exception("Invalid format of public key.");
                lines.RemoveAt(0);
                lines.RemoveAt(lines.Count - 1);
                key = "";
                foreach (var str in lines)
                {
                    key += str;
                }
            }
            key = key.Replace("\r", "").Replace("\n", "");
            return Convert.FromBase64String(key);
        }

        public static byte[] RSAEncrypt(byte[] data,string publicKey)
        {
            var keyData = ParseRSAKey(publicKey);
            var rsa = RSA.Create();
            rsa.FromX509PublicKey(keyData);
            return rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }
        public static byte[] RSAEncrypt(byte[] data, RSA rsa)
        {
            return rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }

        public static byte[] RSADecrypt(byte[] data,string privateKey)
        {
            var keyData = ParseRSAKey(privateKey);
            var rsa = RSA.Create();
            rsa.FromPKCS1PrivateKey(keyData);
            return rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
        }
        public static byte[] RSADecrypt(byte[] data, RSA rsa)
        {
            return rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
        }

        public static string RSABlockEncrypt(byte[] data,string publicKey)
        {
            var keyData = ParseRSAKey(publicKey);
            var rsa = RSA.Create();
            rsa.FromX509PublicKey(keyData);
            var length = rsa.KeySize / 8 - 11;
            var dataBase64 = Convert.ToBase64String(data);
            var dataEnc = "";
            for(var idx = 0; idx < dataBase64.Length; idx += length)
            {
                if (idx + length >= dataBase64.Length)
                    length = dataBase64.Length - idx;
                var dataBlock = dataBase64.Substring(idx, length);
                dataEnc += ("|" + Convert.ToBase64String(RSAEncrypt(Encoding.UTF8.GetBytes(dataBlock), rsa)));
            }
            return dataEnc;
        }

        public static byte[] RSABlockDecrypt(string data,string privateKey)
        {
            return null;
        }

        public static async Task<string> JSONStringifyAsync(object obj)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            using (StreamReader sr = new StreamReader(ms))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jsonSerializer.Serialize(jw, obj);
                await jw.FlushAsync();
                ms.Position = 0;
                var jsonText = sr.ReadToEnd();
                return jsonText;
            }
        }

        public static bool VerifyPasswordHash(string hashIn, string saltTmp)
        {
            var hashStorage = "46c5ec1aa56213d95df984eecf454d40c452bcadf453ff6b447206f96b4b76d36fe971de6fa09254b15f738357abf1f08abac642863e0d6b9186be6f876ef87f";
            var hash = SHA512(hashStorage + saltTmp);
            return hash == hashIn;
        }

        public static string SHA512(string text)
        {
            var sha512 = System.Security.Cryptography.SHA512.Create();
            var hash = BitConverter.ToString(sha512.ComputeHash(Encoding.UTF8.GetBytes(text)));
            hash = hash.Replace("-", "").ToLower();
            return hash;
        }

        public static long GetTimeStampNow()
        {
            long timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            return timestamp;
        }



    }

    
}
