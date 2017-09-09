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
    public class FriendsController : Controller
    {
        // GET: api/friends
        [HttpGet]
        public object Get()
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            return new object[]
            {
                new
                {
                    uid="jack",
                    nickName="Jack",
                    group="default"
                },
                new
                {
                    uid="cherry",
                    nickName="CherrY",
                    group="default"
                },
                new
                {
                    uid="BROWN",
                    nickName="Miku",
                    group="default"
                },
                new
                {
                    uid="Dwscdv3",
                    nickName="Dwscdv3",
                    group="developer"
                },
                new
                {
                    uid="SardineeeFish",
                    nickName="SardineFish",
                    group="developer"
                },
            };
        }

        [HttpGet("{uid}")]
        public object Get(string uid)
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            return new
            {
                code = 0,
                data = ""
            };
        }

        [HttpPost("{uid}")]
        public object Post(string uid, [FromBody]string data)
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            string group = "default";
            try
            {
                var jobj = JObject.Parse(data);
                group = jobj["group"].ToString();
            }
            catch (Exception ex)
            {
                return new
                {
                    code = -100,
                    data = ex.Message
                };
            }

            if(TMCSTest.rand.NextDouble()<0.33)
            {
                return new
                {
                    code = -202,
                    data = new
                    {
                        uid=uid,
                        group=group
                    }
                };
            }
            else if (TMCSTest.rand.NextDouble()<0.5)
            {
                return new
                {
                    code = -301,
                    data = new
                    {
                        uid = uid,
                        group = group
                    }
                };
            }
            else
            {
                return new
                {
                    code = 0,
                    data = new
                    {
                        uid = uid,
                        group = group
                    }
                };
            }
        }

        [HttpPut("{uid}")]
        public object Put(string uid)
        {
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            if (TMCSTest.rand.NextDouble() < 0.25)
            {
                return new
                {
                    code = 0,
                    data = uid
                };
            }
            else if (TMCSTest.rand.NextDouble() < 0.33)
            {
                return new
                {
                    code = -202,
                    data = uid
                };
            }
            else if (TMCSTest.rand.NextDouble() < 0.5)
            {
                return new
                {
                    code = -302,
                    data = uid
                };
            }
            else
            {
                return new
                {
                    code = -303,
                    data = "对方拒绝和你成为朋友，并向你抛出了一个异常."
                };
            }
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
                    code = -202,
                    data = uid
                };
            }
            else
            {
                return new
                {
                    code = -301,
                    data = uid
                };
            }
        }
    }
}
