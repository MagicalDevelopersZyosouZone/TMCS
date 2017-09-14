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
    public class ContactController : Controller
    {
        // GET: api/friends
        [HttpGet]
        public object Get()
        {
            TMCSTest.CORS(Request, Response);
            if (!Request.Cookies.Keys.Contains("token") || Request.Cookies["token"] == "")
            {
                return new
                {
                    code = -210,
                    data = "Please login."
                };
            }
            if (TMCSTest.rand.NextDouble() < 0.05)
            {
                return new
                {
                    code = -210,
                    data = "Please login"
                };
            }
            else
            {
                return new
                {
                    code = 0,
                    data = new object[]
                    {
                        new
                        {
                            uid="jack",
                            nickName="Jack",
                            group="default",
                            note="",
                            profile=TMCSTest.GetUserProfile("jack")
                        },
                        new
                        {
                            uid="cherry",
                            nickName="CherrY",
                            group="default",
                            note="",
                            profile=TMCSTest.GetUserProfile("cherry")
                        },
                        new
                        {
                            uid="BROWN",
                            nickName="Miku",
                            group="default",
                            note="Hentai",
                            profile=TMCSTest.GetUserProfile("BROWN")
                        },
                        new
                        {
                            uid="Dwscdv3",
                            nickName="Dwscdv3",
                            group="developer",
                            note="Dwscdv3",
                            profile=TMCSTest.GetUserProfile("Dwscdv3")
                        },
                        new
                        {
                            uid="SardineFish",
                            nickName="SardineeeFish",
                            group="developer",
                            note="SardineFish",
                            profile=TMCSTest.GetUserProfile("SardineFish")
                        },
                    }
                };
            }
            
        }

        [HttpGet("{uid}")]
        public object Get(string uid)
        {
            TMCSTest.CORS(Request, Response);
            return new
            {
                code = 0,
                data = ""
            };
        }

        [HttpPost("{uid}")]
        public object Post(string uid, [FromBody]dynamic data)
        {
            TMCSTest.CORS(Request, Response);
            string group = "default";
            try
            {
                group = data.group.ToString();
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
            TMCSTest.CORS(Request, Response);
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

        [HttpOptions]
        public void Options()
        {
            TMCSTest.CORS(Request, Response);
        }
    }
}
