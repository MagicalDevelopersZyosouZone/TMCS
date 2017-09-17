using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCS_Test.Controllers
{
    [Route("api/login/password")]
    public class LoginPasswordController : Controller
    {
        // GET: api/values
        [HttpPost]
        public object Post([FromBody]dynamic data)
        {
            TMCSTest.CORS(Request, Response);
            string uid = "";
            string hash = "";
            
            try
            {
                uid = data.uid.ToString();
                hash = data.hash.ToString();

            }
            catch (Exception ex)
            {
                return new
                {
                    code = -100,
                    data = ex.Message
                };
            }
            if (TMCSTest.rand.NextDouble()<0.1)
            {
                return new
                {
                    code = -202,
                    data = new { uid = uid, hash = hash }
                };
            }
            else if (TMCSTest.rand.NextDouble() < 0.8)
            {
                Response.Cookies.Append("token", "I'm a token.");
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        hash = hash,
                        token = "I'm a token.",
                        prvKey = "I'm a private key."
                    }
                };
            }
            else
            {
                return new { code = -201, data = new { uid = uid, hash = hash } };
            }
        }

        [HttpOptions]
        public void Options()
        {
            TMCSTest.CORS(Request, Response);
        }
    }
}
