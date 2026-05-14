using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class ProcessCostingDetails
    {
        [Key]
        public long ID { get; set; }

        public string VendorCode { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public string Unit { get; set; }

        public decimal GoldCharges { get; set; }

        public decimal PlatinumCharges { get; set; }

        public decimal SilverCharges { get; set; }

        public bool IsOptional { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }
    }
}