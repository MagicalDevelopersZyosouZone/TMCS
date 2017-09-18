using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System.Text;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCS_Test.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        public IMemoryCache MemCache;
        public LoginController(IMemoryCache memCache)
        {
            MemCache = memCache;
        }

        // GET: api/values
        [HttpGet]
        public void Get()
        {
            TMCSTest.CORS(Request, Response);
            Response.StatusCode = 403;
        }

        [HttpGet("{uid}")]
        public object Get(string uid)
        {
            TMCSTest.CORS(Request, Response);
            Response.Headers["Cache-Control"] = "no-cache";
            var authCode= Convert.ToBase64String(
                TMCSTest.RSAEncrypt(
                    Encoding.UTF8.GetBytes(TMCSTest.AUTH_CODE),
                    TMCSTest.PUBLIC_KEY));
            if (TMCSTest.rand.NextDouble() < 0.01)
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
                    code = 0,
                    data = new
                    {
                        authType = "Password",
                        salt = TMCSTest.SALT,
                        authCode = authCode,
                        prvKeyEnc = TMCSTest.ENCRYPTED_PRIVATE_KEY
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
                        authType = "PrivateKey",
                        authCode = authCode
                    }
                };
            }
        }
    }
}
