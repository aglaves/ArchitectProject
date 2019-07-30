using ArchitectProject.Controllers;
using ArchitectProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace ArchitectProjectTests.Controllers
{
    public class SitesControllerTest
    {
        [Fact]
        public void TestGetSiteByNPIWithValue()
        {
            EntityContext entityContext = CreateEntityContext();
            loadEntityContextWithData(entityContext);

            SitesController sitesController = new SitesController(entityContext, CreateLogger());
            Task<ActionResult<Site>> result = sitesController.GetSiteByNPI("1234567890");
            var actionResult = Assert.IsType<ActionResult<Site>>(result.Result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var model = Assert.IsType<Site>(okResult.Value);
            Assert.Equal("1234567890", model.NPI);
        }

        [Fact]
        public void TestAddSiteSetsNPIRouteValue()
        {
            EntityContext entityContext = CreateEntityContext();
            loadEntityContextWithData(entityContext);

            SitesController sitesController = new SitesController(entityContext, CreateLogger());
            Task<ActionResult<Site>> result = sitesController.AddSite(CreateSite("999888777", "Site for Add Test"));
            var actionResult = Assert.IsType<ActionResult<Site>>(result.Result);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            object resultNPI;
            createdAtResult.RouteValues.TryGetValue("NPI", out resultNPI);
            Assert.Equal("999888777", resultNPI);
        }

        [Fact]
        public void TestAddSiteSetsSiteInResult()
        {
            EntityContext entityContext = CreateEntityContext();
            loadEntityContextWithData(entityContext);

            SitesController sitesController = new SitesController(entityContext, CreateLogger());
            Task<ActionResult<Site>> result = sitesController.AddSite(CreateSite("999888777", "Site for Add Test"));
            var actionResult = Assert.IsType<ActionResult<Site>>(result.Result);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var site = Assert.IsType<Site>(createdAtResult.Value);
            Assert.Equal("999888777", site.NPI);
            Assert.Equal("Site for Add Test", site.SiteName);
        }

        [Fact]
        public async void TestAddSiteCreatesSite()
        {
            EntityContext entityContext = CreateEntityContext();
            loadEntityContextWithData(entityContext);

            SitesController sitesController = new SitesController(entityContext, CreateLogger());
            await sitesController.AddSite(CreateSite("999888777", "Site for Add Test"));
            Site site = entityContext.SiteItems.Find("999888777");
            Assert.Equal("999888777", site.NPI);
            Assert.Equal("Site for Add Test", site.SiteName);
        }

        private EntityContext CreateEntityContext()
        {
            DbContextOptions<EntityContext> options;
            var builder = new DbContextOptionsBuilder<EntityContext>();
            builder.UseInMemoryDatabase();
            options = builder.Options;
            EntityContext personDataContext = new EntityContext(options);
            personDataContext.Database.EnsureDeleted();
            personDataContext.Database.EnsureCreated();
            return personDataContext;
        }

        private void loadEntityContextWithData(EntityContext entityContext)
        {
            entityContext.Add(CreateSite("1234567890", "First Test Site"));
            entityContext.Add(CreateSite("0987654321", "Second Test Site"));
            entityContext.Add(CreateSite("111222333", "Third Test Site"));
            entityContext.Add(CreateSite("444555666", "Fourth Test Site"));
            entityContext.Add(CreateSite("777777777", "Fifth Test Site"));
            entityContext.SaveChanges();
        }

        private Site CreateSite(string npi, string siteName)
        {
            Site site = new Site
            {
                NPI = npi,
                SiteName = siteName
            };
            return site;
        }

        private ILogger<SitesController> CreateLogger()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<SitesController>();

            return logger;
        }
    }
}
