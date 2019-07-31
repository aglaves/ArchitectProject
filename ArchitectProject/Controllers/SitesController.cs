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

            Site site = await entityContext.Sites.FindAsync(npi);
            if (site == null)
                return NotFound();
            return Ok(site);
        }

        [HttpPost]
        public async Task<ActionResult<Site>> AddSite(Models.Site site)
        {
            if (site == null)
            {
                logger.LogDebug("Site is null.");
                return NoContent();
            }
            else
            {
                logger.LogDebug("Site NPI: " + site.NPI);
                logger.LogDebug("Site SiteName: " + site.SiteName);
                if (entityContext.Find<Site>(site.NPI) == null)
                    entityContext.Add(site);
                else
                    entityContext.Update(site);
                await entityContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSiteByNPI), new { site.NPI }, site);
            }    
        }

        [HttpGet("{npi}/inventory-summary", Name = "InventorySummary")]
        public ActionResult<InventorySummary> GetInventorySummary(string npi) {
            logger.LogDebug("Inventory summary request for {0}", npi);
            logger.LogTrace("Total number of inventory summary details: {0}", entityContext.InventorySummaries.Count);
            Site site = entityContext.Find<Site>(npi);
            InventorySummary inventorySummaryDetails;
            if (entityContext.InventorySummaries.ContainsKey(site))
            {
                logger.LogDebug("Found Inventory Summary Detail.");
                inventorySummaryDetails = entityContext.InventorySummaries[site];
            }
            else
            {
                logger.LogDebug("No Inventory Summary Detail for site.");
                inventorySummaryDetails = new InventorySummary();
            }
            return Ok(inventorySummaryDetails);
        }
    }
}
