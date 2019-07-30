using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ArchitectProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly EntityContext entityContext;

        public SitesController(EntityContext entityContext, ILogger<SitesController> logger)
        {
            this.logger = logger;
            this.entityContext = entityContext;
        }

        [HttpGet("{npi}", Name = "GetSiteByNPI")]
        public async Task<ActionResult<Site>> GetSiteByNPI(string npi)
        {
            logger.LogDebug("Received request for site: {npi}", npi);
            if (npi == null) {
                logger.LogInformation("Site NPI is null.");
                return NoContent();
            }

            Site site = await entityContext.SiteItems.FindAsync(npi);
            if (site == null)
                return NotFound();
            return Ok(site);
        }

        [HttpPost]
        public async Task<ActionResult<Site>> AddSite(Models.Site site)
        {
            if (site == null)
            {
                logger.LogInformation("Site is null.");
                return NoContent();
            }
            else
            {
                logger.LogInformation("Site NPI: " + site.NPI);
                logger.LogInformation("Site SiteName: " + site.SiteName);
                entityContext.Add(site);
                await entityContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSiteByNPI), new { site.NPI }, site);
            }    
        }
    }
}
