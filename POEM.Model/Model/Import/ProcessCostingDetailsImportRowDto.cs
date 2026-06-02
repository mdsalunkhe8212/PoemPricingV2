using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class ProcessCostingDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }
        public string VendorCode { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        public decimal GoldCharges { get; set; }
        public decimal PlatinumCharges { get; set; }
        public decimal SilverCharges { get; set; }
        public bool IsOptional { get; set; }
    }
}