using System;
using System.Threading.Tasks;
using ArchitectProject.Exceptions;
using ArchitectProject.Http;
using ArchitectProject.Models;
using ArchitectProject.Parsers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ArchitectProject.Controllers
{

    [Route("api/[controller]/qoh")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly EntityContext entityContext;
        private static Task runningTask;
        private readonly QuantityOnHandFileParser quantityOnHandFileParser;
        private readonly HttpRequestFileExtractor httpRequestFileExtractor;

        public FilesController(EntityContext entityContext, ILogger<FilesController> logger, QuantityOnHandFileParser quantityOnHandFileParser, HttpRequestFileExtractor httpRequestFileExtractor)
        {
            this.logger = logger;
            this.entityContext = entityContext;
            this.quantityOnHandFileParser = quantityOnHandFileParser;
            this.httpRequestFileExtractor = httpRequestFileExtractor;
        }

        [HttpPost]
        public ActionResult<FileDetail> UploadQuantityOnHand()
        {
            FileExtractionResult fileExtractionResult = null;
            try
            {
                fileExtractionResult = httpRequestFileExtractor.ExtractDataFromRequest(Request);
            }
            catch (InvalidRequestException e)
            {
                return BadRequest(e.Message);
            }
            logger.LogTrace("Created detail file record.");
            runningTask = Task.Run(() => quantityOnHandFileParser.processQuantityOnHandFileAsynchronously(fileExtractionResult));
            return CreatedAtAction(nameof(RetrieveStatus), new { fileExtractionResult.FileDetail.Id }, fileExtractionResult.FileDetail);
        }

        [HttpGet("{id}", Name = "RetrieveStatus")]
        public ActionResult<FileDetail> RetrieveStatus(Guid id)
        {
            logger.LogTrace("Requesting id: " + id);
            FileDetail quantityOnHandFileDetail = entityContext.Find<FileDetail>(id);
            if (quantityOnHandFileDetail == null)
                return NotFound();
            else
                return Ok(quantityOnHandFileDetail);
        }        
    }
}