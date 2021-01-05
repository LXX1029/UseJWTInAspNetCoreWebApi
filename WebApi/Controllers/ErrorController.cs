using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("/error")]
        public IActionResult Index()
        {
            return new ContentResult() { Content = "exception" };
        }
    }
}
