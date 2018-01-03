using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fanex.RemotingRef.Service.Demo.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;

namespace Fanex.RemotingRef.Service.Demo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/nexcel/remotingref/1.0.0/inventory")]
        [SwaggerOperation("SearchInventory")]
        [SwaggerResponse(200, typeof(List<string>), "search results matching criteria")]
        [SwaggerResponse(400, typeof(List<string>), "bad input parameter")]
        public IActionResult Index()
        {
            string exampleJson = null;
            var defaultValue = new List<string> { "test" };
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<string>>(exampleJson)
            : defaultValue;
            return new ObjectResult(example);
        }
    }
}