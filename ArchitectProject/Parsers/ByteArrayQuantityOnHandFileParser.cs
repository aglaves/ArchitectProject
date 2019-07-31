using ArchitectProject.Http;
using ArchitectProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectProject.Parsers
{
    public class ByteArrayQuantityOnHandFileParser : QuantityOnHandFileParser
    {
        private static readonly int NPI_POSITION = 0;
        private static readonly int NDC_POSITION = 1;
        private static readonly int TOTAL_UNITS_POSITION = 2;
        private static readonly int UNIT_COST_POSITION = 3;
        private readonly ILogger logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private EntityContext entityContext = null;

        public ByteArrayQuantityOnHandFileParser(ILogger<QuantityOnHandFileParser> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Guid> processQuantityOnHandFileAsynchronously(FileExtractionResult fileExtractionResult)
        {
            logger.LogInformation("Starting processing of file.");
            using (var scope = serviceScopeFactory.CreateScope())
            {
                entityContext = scope.ServiceProvider.GetRequiredService<EntityContext>();

                uint validLines = 0;
                uint invalidLines = 0;
                Boolean headerLine = true;
                string qoh = Encoding.UTF8.GetString(fileExtractionResult.FileContent, 0, fileExtractionResult.FileContent.Length);
                string[] lines = qoh.Split("\n");
                foreach (string line in lines)
                {
                    if (headerLine)
                    {
                        headerLine = false;
                        logger.LogInformation("Skipping header.");
                    }
                    else if (line.Length == 0)
                    {
                        logger.LogInformation("Skipping blank line.");
                        continue;
                    }
                    else
                    {
                        if (ProcessDetailLine(line))
                            validLines++;
                        else
                            invalidLines++;
                    }
                }
                logger.LogInformation("Completed processing file.");
                UpdateFileDetailToCompleted(fileExtractionResult.FileDetail, validLines, invalidLines);
                logger.LogInformation("Quantity On Hand File Details updated.");
                logger.LogInformation("Number of Inventory Summaries: {0}", entityContext.InventorySummaries.Count);
                int x = await entityContext.SaveChangesAsync();
                return fileExtractionResult.FileDetail.Id;
            }
        }

        private Boolean ProcessDetailLine(string detailLine)
        {
            string[] lineFields = detailLine.Split("|");
            if (!ValidLine(lineFields))
            {
                logger.LogWarning("Invalid line: {0}", detailLine);
                return false;
            }
            SiteItem siteItem = CreateSiteItem(lineFields);
            HandleNewSiteItem(siteItem, entityContext);
            return true;
        }

        private Boolean ValidLine(string[] lineFields)
        {
            if (!CorrectNumberOfFields(lineFields))
                return false;

            if (!ValidSiteNumber(lineFields[NPI_POSITION]))
                return false;

            if (!ValidTotalUnits(lineFields[TOTAL_UNITS_POSITION]))
                return false;

            if (!ValidUnitCost(lineFields[UNIT_COST_POSITION]))
                return false;

            return true;
        }

        private Boolean CorrectNumberOfFields(string[] lineFields)
        {
            if (lineFields.Length != 4)
            {
                return false;
            }
            return true;
        }

        private Boolean ValidSiteNumber(string siteNumber)
        {
            Site site = entityContext.Sites.Find(siteNumber);
            if (site == null)
            {
                return false;
            }
            return true;
        }

        private Boolean ValidTotalUnits(string totalUnits)
        {
            try
            {
                System.Convert.ToInt32(totalUnits);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            return true;
        }

        private Boolean ValidUnitCost(string unitCost)
        {
            try
            {
                System.Convert.ToDecimal(unitCost);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            return true;
        }

        private SiteItem CreateSiteItem(string[] lineFields)
        {
            SiteItem item = new SiteItem
            {
                NPI = lineFields[NPI_POSITION],
                NDC = lineFields[NDC_POSITION],
                TotalUnits = System.Convert.ToInt32(lineFields[TOTAL_UNITS_POSITION]),
                UnitCost = System.Convert.ToDecimal(lineFields[UNIT_COST_POSITION])
            };
            entityContext.Add(item);
            return item;
        }
        private void HandleNewSiteItem(SiteItem siteItem, EntityContext entityContext)
        {
            Site site = entityContext.Find<Site>(siteItem.NPI);
            InventorySummary inventorySummary = null;
            if (entityContext.InventorySummaries.ContainsKey(site))
            {
                inventorySummary = entityContext.InventorySummaries[site];
            }
            else
            {
                logger.LogInformation("No inventory site found.  Creating a new one.");
                inventorySummary = new InventorySummary();
                entityContext.InventorySummaries.Add(site, inventorySummary);
            }
            inventorySummary.AddSiteItem(siteItem);

        }

        private void UpdateFileDetailToCompleted(FileDetail fileDetail, uint validLineCount, uint invalidLineCount)
        {
            fileDetail.Status = Models.FileStatus.Completed;
            fileDetail.InvalidRecordCount = invalidLineCount;
            fileDetail.ValidRecordCount = validLineCount;
            fileDetail.TotalRecordCount = invalidLineCount + validLineCount;
            entityContext.Update(fileDetail);
        }
    }
}
