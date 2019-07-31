using System.ComponentModel.DataAnnotations;

namespace ArchitectProject.Models
{
    public class SiteItem
    {
        [Key]
        public string NPI { get; set; }
        [Key]
        public string NDC { get; set; }
        public int TotalUnits { get; set; }
        public decimal UnitCost { get; set; }

        public decimal TotalValue { get { return TotalUnits * UnitCost; } }
    }
}
