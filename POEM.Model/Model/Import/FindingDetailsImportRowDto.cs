using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class FindingDetailsImportRowDto : BaseImportRowDto
    {
        public string FindingSupplier { get; set; }

        public string FindingVendorName { get; set; }

        public string FindingVendorCode { get; set; }


        public string Company { get; set; }
        public string FindingNumber { get; set; }
        public string FindingMetalType { get; set; }
        public string FindingMetalKt { get; set; }
        public string FindingMetalColor { get; set; }
        public string FindingType { get; set; }
        public string FindingDescription { get; set; }

        public string FindingShortDescription { get; set; }

        public decimal PerPcFindingWeightGms { get; set; }
        public decimal Increment { get; set; }
        public decimal Decrement { get; set; }

        public int MetalLock { get; set; }

        public decimal FindingCost { get; set; }

        

    }
}