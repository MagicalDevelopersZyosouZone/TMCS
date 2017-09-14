using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCS_Test.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet]
        public void Get()
        {
            TMCSTest.CORS(Request, Response);
            Response.StatusCode = 403;
        }

        // GET api/user/Dwscdv3
        [HttpGet("{uid}")]
        public object Get(string uid)
        {
            Response.Headers["Cache-Control"] = "no-cache";
            TMCSTest.CORS(Request, Response);
            if (TMCSTest.rand.Next() < 0.1 || !Request.Cookies.Keys.Contains("token"))
            {
                return new
                {
                    code = -210,
                    data = new
                    {
                        uid = uid
                    }
                };
            }
            if (TMCSTest.rand.NextDouble() < 0.1)
            {
                return new
                {
                    code = -202,
                    data = uid
                };
            }
            else
            {
                return new
                {
                    code = 0,
                    data = TMCSTest.GetUserProfile(uid)
                };
            }
        }

        [HttpPost("{uid}")]
        public object Post(string uid, [FromBody]string data)
        {
            TMCSTest.CORS(Request, Response);
            if (TMCSTest.rand.NextDouble()<0.5)
            {
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        data = data
                    }
                };
            }
            else
            {
                return new
                {
                    code = -210,
                    data = new
                    {
                        uid = uid,
                        data = data
                    }
                };
            }
        }

        [HttpPut("{uid}")]
        public object Put(string uid, [FromBody]dynamic data)
        {
            TMCSTest.CORS(Request, Response);
            string pubKey = "";
            try
            {
                pubKey = data.pubKey.ToString();
            }
            catch (Exception ex)
            {
                return new
                {
                    code = -100,
                    data = ex.Message
                };
            }
            if (TMCSTest.rand.NextDouble()<0.5)
            {
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        pubKey = pubKey
                    }
                };
            }
            else
            {
                return new
                {
                    code = -203,
                    data = new
                    {
                        uid = uid,
                        pubKey = pubKey
                    }
                };

            };
        }

        [HttpDelete("{uid}")]
        public object Delete(string uid)
        {
            TMCSTest.CORS(Request, Response);
            if (TMCSTest.rand.NextDouble()<0.33)
            {
                return new
                {
                    code = 0,
                    data = uid
                };
            }
            else if (TMCSTest.rand.NextDouble()<0.5)
            {
                return new
                {
                    code = -210,
                    data = uid
                };
            }
            else
            {
                return new
                {
                    cod = -202,
                    data = uid
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
