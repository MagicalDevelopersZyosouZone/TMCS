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
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            Response.StatusCode = 403;
        }

        // GET api/user/Dwscdv3
        [HttpGet("{uid}")]
        public object Get(string uid)
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            if (TMCSTest.rand.NextDouble() < 0.33)
            {
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        nickName = uid.ToUpper(),
                        sex = "Male",
                        status = "Online",
                        avatar = "http://img.sardinefish.com/NDc2NTU2"
                    }
                };
            }
            else if (TMCSTest.rand.NextDouble() < 0.5)
            {
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        nickName = uid.ToUpper(),
                        sex = "Female",
                        status = "Offline",
                        avatar = "http://img.sardinefish.com/NDc2NTU2"
                    }
                };
            }
            else
            {
                return new
                {
                    code = -202,
                    data = uid
                };
            }
        }

        [HttpPost("{uid}")]
        public object Post(string uid, [FromBody]string data)
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
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
        public object Put(string uid, [FromBody]string data)
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            string pubKey = "";
            try
            {
                var jobj = JObject.Parse(data);
                uid = jobj["uid"].ToString();
                pubKey = jobj["publicKey"].ToString();
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
            Response.Headers["Access-Control-Allow-Origin"] = "*";
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

    }
}
