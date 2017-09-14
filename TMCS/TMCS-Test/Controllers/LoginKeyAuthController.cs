using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCS_Test.Controllers
{
    [Route("api/login/key-auth")]
    public class LoginKeyAuthController : Controller
    {
        [HttpPost]
        public object Post([FromBody]dynamic data)
        {
            TMCSTest.CORS(Request, Response);
            string uid = "", authCode = "";
            try
            {
                uid = data.uid.ToString();
                authCode = data.authCode.ToString();
            }
            catch (Exception ex)
            {
                return new
                {
                    code = -100,
                    data = ex.Message
                };
            }
            if(TMCSTest.rand.NextDouble()<0.1)
            {
                return new
                {
                    code = -202,
                    data = new { uid = uid, authCode = authCode }
                };
            }
            else if (TMCSTest.rand.NextDouble()<0.8)
            {
                
                Response.Cookies.Append("token", "I'm a token.");
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        authCode = authCode,
                        token= "I'm a token."
                    }
                };
            }
            else
            {
                return new
                {
                    code = -201,
                    data = new
                    {
                        uid = uid,
                        authCode = authCode
                    }
                };
            }
        }

        [HttpOptions]
        public void Options()
        {
            TMCSTest.CORS(Request, Response);
        }
    }
}
