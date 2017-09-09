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
        public object Post([FromBody]string data)
        {
            Response.Headers["Access-Control-Allow-Headers"] = "content-type";
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            var jobj = JObject.Parse(data);
            string uid = jobj["uid"].ToString();
            string hash = jobj["hash"].ToString();
            if (TMCSTest.rand.NextDouble()<0.33)
            {
                return new
                {
                    code = -202,
                    data = new { uid = uid, hash = hash }
                };
            }
            else if (TMCSTest.rand.NextDouble() < 0.5)
            {
                return new { code = 0, data = new { uid = uid, hash = hash } };
            }
            else
            {
                return new { code = -201, data = new { uid = uid, hash = hash } };
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
