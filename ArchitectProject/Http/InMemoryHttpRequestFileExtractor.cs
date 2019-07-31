using ArchitectProject.Exceptions;
using ArchitectProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace ArchitectProject.Http
{
    public class InMemoryHttpRequestFileExtractor : HttpRequestFileExtractor
    {
        private EntityContext entityContext;
        private ILogger logger;

        public InMemoryHttpRequestFileExtractor(EntityContext entityContext, ILogger<HttpRequestFileExtractor> logger)
        {
            this.entityContext = entityContext;
            this.logger = logger;
        }

        public FileExtractionResult ExtractDataFromRequest(HttpRequest request)
        {
            if (!ValidUploadRequest(request))
                throw new InvalidRequestException("No valid file in request.");
            IFormFile formFile = ExtractFormFile(request);
            byte[] uploadFileContent = ParseRequestForDataFile(formFile);
            FileDetail fileDetail = CreateInprocessFileDetail(formFile.FileName);
            return new FileExtractionResult {
                FileContent = uploadFileContent,
                FileDetail = fileDetail

            };
        }

        private Boolean ValidUploadRequest(HttpRequest request)
        {
            return !((request.Form.Files == null || !request.Form.Files.Any()));
        }

        private IFormFile ExtractFormFile(HttpRequest request)
        {
            return request.Form.Files.First();
        }

        private byte[] ParseRequestForDataFile(IFormFile formFile)
        {

            byte[] fileContent;

            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                fileContent = new byte[memoryStream.Length];
                memoryStream.Read(fileContent, 0, fileContent.Length);
            }

            return fileContent;
        }

        private FileDetail CreateInprocessFileDetail(String fileName)
        {
            FileDetail fileDetail = new FileDetail();
            fileDetail.FileName = fileName;
            fileDetail.Status = Models.FileStatus.Processing;
            entityContext.Add(fileDetail);
            entityContext.SaveChanges();
            logger.LogDebug("Guid created: " + fileDetail.Id);
            return fileDetail;
        }
    }
}
