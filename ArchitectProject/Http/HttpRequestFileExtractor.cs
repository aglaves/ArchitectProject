using ArchitectProject.Exceptions;
using ArchitectProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArchitectProject.Http
{
    public interface HttpRequestFileExtractor
    {

        FileExtractionResult ExtractDataFromRequest(HttpRequest request);
    }
}
