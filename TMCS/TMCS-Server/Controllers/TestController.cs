using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMCS.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("GET");
        }

        [HttpPost]
        public IActionResult Post([FromBody] dynamic obj)
        {
            if (Common.HasProperty(obj, "echo"))
            {
                return Ok(obj.echo);
            }
            else
            {
                return Ok("POST");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] dynamic obj)
        {
            if (obj.echo != null)
            {
                return Ok(obj.echo);
            }
            else
            {
                return Ok("PUT");
            }
        }
    }
}