using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitController
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "All systems are go!";
        }
    }
}
