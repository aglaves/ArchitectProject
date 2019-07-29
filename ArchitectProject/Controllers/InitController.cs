using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public ActionResult<string> Status()
        {
            return Ok("All systems are go!");
        }
    }
}
