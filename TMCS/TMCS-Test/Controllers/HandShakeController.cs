using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMCS_Test.Controllers
{
    [Route("api/[controller]")]
    public class HandShakeController : Controller
    {
        // GET: api/handshake
        [HttpGet]
        public object Get()
        {
            TMCSTest.CORS(Request,Response);
            Response.Headers["Cache-Control"] = "no-cache";
            return new
            {
                code = 0,
                data = new
                {
                    serverName = "TMCS-Test",
                    owner = "MDZZ.studio",
                    version = "0.1.0-alpha",
                    pubKey = TMCSTest.PUBLIC_KEY
                }
            };
        }
    }
}
