using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArchitectProject.Models
{
    public class Site
    {
        [Key]
        public string NPI { get; set; }

        public string SiteName { get; set; }

        public override bool Equals(object obj)
        {
            var site = obj as Site;
            return site != null &&
                   NPI == site.NPI &&
                   SiteName == site.SiteName;
        }

        public override int GetHashCode()
        {
            var hash = 13;

            hash += 7 * NPI.GetHashCode();
            hash += 7 * SiteName.GetHashCode();

            return hash;
        }

        public static bool operator ==(Site site1, Site site2)
        {
            return EqualityComparer<Site>.Default.Equals(site1, site2);
        }

        public static bool operator !=(Site site1, Site site2)
        {
            return !(site1 == site2);
        }
    }
}
