using System.Collections.Generic;

namespace ArchitectProject.Models
{
    public class InventorySummary
    {
        private readonly List<SiteItem> siteItems = new List<SiteItem>();

        public uint UniqueDrugCount { get { return (uint) siteItems.Count; } }

        public decimal TotalInventoryValue
        {
            get {
                decimal totalInventoryValue = 0;
                foreach (SiteItem siteItem in siteItems)
                    totalInventoryValue += siteItem.UnitCost * siteItem.TotalUnits;
                return totalInventoryValue;
            }
        }

        public SiteItem HighestUnitCostSiteItem
        {
            get
            {
                SiteItem highestUnitCostSiteItem = null;
                foreach (SiteItem siteItem in siteItems)
                    if (highestUnitCostSiteItem == null || siteItem.UnitCost > highestUnitCostSiteItem.UnitCost)
                        highestUnitCostSiteItem = siteItem;
                    else if (siteItem.UnitCost == highestUnitCostSiteItem.UnitCost)
                        if (System.Convert.ToInt32(siteItem.NDC) > System.Convert.ToInt32(highestUnitCostSiteItem.NDC))
                            highestUnitCostSiteItem = siteItem;
                return highestUnitCostSiteItem;
            }
        }

        public SiteItem HighestInventoryUnitsSiteItem
        {
            get
            {
                SiteItem highestUnits = null;
                foreach (SiteItem siteItem in siteItems)
                    if (highestUnits == null || siteItem.TotalUnits > highestUnits.TotalUnits)
                        highestUnits = siteItem;
                    else if (siteItem.TotalUnits == highestUnits.TotalUnits)
                        if (System.Convert.ToInt32(siteItem.NDC) > System.Convert.ToInt32(highestUnits.NDC))
                            highestUnits = siteItem;
                return highestUnits;
            }
        }

        public SiteItem HighestInventoryValueSiteItem
        {
            get
            {
                SiteItem highestInventoryValue = null;
                foreach (SiteItem siteItem in siteItems)
                    if (highestInventoryValue == null || (siteItem.TotalValue) > (highestInventoryValue.TotalValue))
                        highestInventoryValue = siteItem;
                    else if ((siteItem.TotalValue) == (highestInventoryValue.TotalValue))
                        if (System.Convert.ToInt32(siteItem.NDC) > System.Convert.ToInt32(highestInventoryValue.NDC))
                            highestInventoryValue = siteItem;
                return highestInventoryValue;
            }
        }

        public SiteItem LowestUnitCostSiteItem
        {
            get
            {
                SiteItem lowestUnitCostSiteItem = null;
                foreach (SiteItem siteItem in siteItems)
                    if (lowestUnitCostSiteItem == null || siteItem.UnitCost < lowestUnitCostSiteItem.UnitCost)
                        lowestUnitCostSiteItem = siteItem;
                    else if (siteItem.UnitCost == lowestUnitCostSiteItem.UnitCost)
                        if (System.Convert.ToInt32(siteItem.NDC) < System.Convert.ToInt32(lowestUnitCostSiteItem.NDC))
                            lowestUnitCostSiteItem = siteItem;
                return lowestUnitCostSiteItem;
            }
        }

        public SiteItem LowestInventoryUnitsSiteItem
        {
            get
            {
                SiteItem lowestInventoryValue = null;
                foreach (SiteItem siteItem in siteItems)
                    if (lowestInventoryValue == null || (siteItem.TotalUnits) < (lowestInventoryValue.TotalUnits))
                        lowestInventoryValue = siteItem;
                    else if ((siteItem.TotalUnits) == (lowestInventoryValue.TotalUnits))
                        if (System.Convert.ToInt32(siteItem.NDC) < System.Convert.ToInt32(lowestInventoryValue.NDC))
                            lowestInventoryValue = siteItem;
                return lowestInventoryValue;
            }
        }

        public SiteItem LowestInventoryValueSiteItem
        {
            get
            {
                SiteItem lowestInventoryValue = null;
                foreach (SiteItem siteItem in siteItems)
                    if (lowestInventoryValue == null || (siteItem.TotalValue) < (lowestInventoryValue.TotalValue))
                        lowestInventoryValue = siteItem;
                    else if ((siteItem.TotalValue) == (lowestInventoryValue.TotalValue))
                        if (System.Convert.ToInt32(siteItem.NDC) < System.Convert.ToInt32(lowestInventoryValue.NDC))
                            lowestInventoryValue = siteItem;
                return lowestInventoryValue;
            }
        }

        public void AddSiteItem(SiteItem siteItem)
        {
            if (siteItems.Contains(siteItem))
                return;
            siteItems.Add(siteItem);
        }
    }
}
