using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMCS_Test
{
    public class TMCSTest
    {
        public static Random rand = new Random();
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
                    avatar = "http://img.sardinefish.com/NDgxMjA3"
                };
            }
            else if (uid == "SardineFish")
            {
                return new
                {
                    uid = uid,
                    nickName = "SardineFish",
                    status = "Online",
                    avatar = "http://img.sardinefish.com/NDgwOTM5"

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
                    avatar = "http://img.sardinefish.com/NDc2NTU2"

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
                    avatar = "http://img.sardinefish.com/NDc2NTU2"
                };
                
            }
        }
    }
}
