using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMCS.Server
{
    public class Account
    {
        public static object Register(dynamic param)
        {
            if (param.HasProperty("userId") &&
                param.HasProperty("publicKey"))
            {

            }
            return new
            {
                errorCode = -100
            };
        }
    }
        public static object Login()
        {

        }

}
