using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectProject.Controllers
{
    [Route("/")]
    [ApiController]
    [AllowAnonymous]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetDefaultPage()
        {
            return Ok();
        }
    }
}