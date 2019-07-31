using ArchitectProject.Models;

namespace ArchitectProject.Http
{
    public class FileExtractionResult
    {
        public byte[] FileContent { get; set; }
        public FileDetail FileDetail { get; set; }
    }
}
