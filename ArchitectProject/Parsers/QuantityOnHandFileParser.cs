
using ArchitectProject.Http;
using System;
using System.Threading.Tasks;

namespace ArchitectProject.Parsers
{
    public interface QuantityOnHandFileParser
    {
        Task<Guid> processQuantityOnHandFileAsynchronously(FileExtractionResult fileExtractionResult);
    }
}
