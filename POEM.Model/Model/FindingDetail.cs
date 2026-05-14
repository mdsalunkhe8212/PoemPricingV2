using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class FindingDetail
	{
        [Key]
        public int FindingId { get; set; }          // Identity PK by convention for int

        public string FindingSupplier { get; set; } = string.Empty;
        public string FindingVendorName { get; set; } = string.Empty;
        public string FindingVendorCode { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string FindingNumber { get; set; } = string.Empty;
        public string FindingMetalType { get; set; } = string.Empty;
        public string FindingMetalKt { get; set; }
        public string FindingMetalColor { get; set; } = string.Empty;
        public string FindingType { get; set; } = string.Empty;
        public string FindingDescription { get; set; } = string.Empty;

        
        public decimal PerPcFindingWeightGms { get; set; }

        
        public decimal Increment { get; set; }

        
        public decimal Decrement { get; set; }

        public int MetalLock { get; set; }

        
        public decimal FindingCost { get; set; }    // money-style 
    }
}