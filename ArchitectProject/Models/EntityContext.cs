using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ArchitectProject.Models
{
    public class EntityContext : DbContext
    {
        private static Dictionary<Site, InventorySummary> inventorySummaries = new Dictionary<Site, InventorySummary>();
        public EntityContext(DbContextOptions<EntityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteItem>()
                .HasKey(item => new { item.NPI, item.NDC });
        }

        public DbSet<Site> Sites { get; set; }

        public DbSet<SiteItem> SiteItems { get; set; }

        public DbSet<FileDetail> FileDetails { get; set; }

        public Dictionary<Site, InventorySummary> InventorySummaries { get { return inventorySummaries; } }
    }
}
