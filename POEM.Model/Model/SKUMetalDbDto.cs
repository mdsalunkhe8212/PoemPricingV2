using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEM.Model.Model
{
    [Table("SKUMetalDetails")]
    public class SKUMetalDbDto
    {
        [Key]
        public long MetalId { get; set; }
        public long SKUId { get; set; }
        public string MetalText { get; set; }
        public string MetalIdText { get; set; }
        public string KaratText { get; set; }
        public string KaratId { get; set; }
        public string ColorText { get; set; }
        public string ColorId { get; set; }

        public decimal GmWt { get; set; }
        public decimal RatePOz { get; set; }
        public decimal RatePerGm { get; set; }
        public decimal MetalCost { get; set; }
        public decimal MetalInc { get; set; }
        public decimal MetalDec { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}