using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchitectProject.Models
{
    public class Site
    {
        [Key]
        public string NPI { get; set; }

        public string SiteName { get; set; }
    }
}
