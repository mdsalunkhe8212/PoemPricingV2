using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class VendorDetails
	{
        [Key]
        public long VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorLocation { get; set; }
        public string VendorName { get; set; }
        public decimal? DiamondHandlingLab { get; set; }
        public decimal? DiaHndLabLow { get; set; }
        public decimal? DiaHndLabHigh { get; set; }
        public decimal? DiamondHandlingMined { get; set; }
        public decimal? DiaHndMinedLow { get; set; }
        public decimal? DiaHndMinedHigh { get; set; }
        public decimal? FindingHndGold { get; set; }
        public decimal? FindingHndPlatinum { get; set; }
        public decimal? FindingHndSilver { get; set; }

        public decimal? ModelMkgGold { get; set; }
        public decimal? ModelMkgPlatinum { get; set; }
        public decimal? ModelMkgSilver { get; set; }

        public decimal? CAMGold { get; set; }
        public decimal? CAMPlatinum { get; set; }
        public decimal? CAMSilver { get; set; }

        public bool ProductVendor { get; set; }
        public bool FindingsSupplier { get; set; }
        public bool FindingsAssembly { get; set; }
        public bool StoneVendor { get; set; }
        public bool SettingVendor { get; set; }

        public bool LabourLocation { get; set; }
    }
}