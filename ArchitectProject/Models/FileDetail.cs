using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchitectProject.Models
{
    public class FileDetail
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public FileStatus Status { get; set; }

        public uint TotalRecordCount { get; set; }

        public uint ValidRecordCount { get; set; }

        public uint InvalidRecordCount { get; set; }
    }
}
