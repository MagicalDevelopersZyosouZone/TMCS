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
        public object Post([FromBody]JObject data)
        {
            Response.Headers["Access-Control-Allow-Headers"] = "content-type";
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            if(TMCSTest.rand.NextDouble()<0.33)
            {
                var jsonOjb = data;
                var uid = jsonOjb["uid"].ToString();
                var authCode = jsonOjb["authCode"].ToString();
                return new
                {
                    code = -202,
                    data = new { uid = uid, authCode = authCode }
                };
            }
            else if (TMCSTest.rand.NextDouble()<0.5)
            {
                var jsonOjb = JObject.Parse(data);
                var uid = jsonOjb["uid"].ToString();
                var authCode = jsonOjb["authCode"].ToString();
                return new { code = 0, data = new { uid = uid, authCode = authCode } };
            }
            else
            {
                var jsonOjb = JObject.Parse(data);
                var uid = jsonOjb["uid"].ToString();
                var authCode = jsonOjb["authCode"].ToString();
                return new { code = -201, data = new { uid = uid, authCode = authCode } };
            }
        }

        [HttpOptions]
        public void Options()
        {
            Response.Headers["Access-Control-Allow-Headers"] = "content-type";
            Response.Headers["Access-Control-Allow-Origin"] = "*";
        }
    }
}
