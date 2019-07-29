using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectProject.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArchitectProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private ILogger logger;

        public SecurityController(IOptions<AppSettings> appSettings, ILogger<SecurityController> logger)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        [Route("newToken")]
        [HttpGet("{username}")]
        [AllowAnonymous]
        public ActionResult<string> Get([FromQuery] string Username)
        {
            logger.LogInformation("Use Guid: " + JwtSecurityKey.IsUseGuid());
            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(appSettings.SecretKey))
                                .AddSubject(Username)
                                .AddIssuer(appSettings.Issuer)
                                .AddAudience(appSettings.Audience)
                                .Build();

            return Ok(token.Value);
        }
    }
}