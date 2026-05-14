using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("SKUFindingsDetails")]
    public class SKUFindingsDbDto
    {
        [Key]
        public long FindingsId { get; set; }
        public long SKUId { get; set; }
        public string Supplier { get; set; }
        public string SupplierId { get; set; }
        public string FindingType { get; set; }
        public string FindingTypeId { get; set; }
        public string FindingDescription { get; set; }
        public string FindingSku { get; set; }

        public string FindingMetal { get; set; }
        public string FindingKarat { get; set; }
        public string FindingColor { get; set; }

        public string FindingAssembly { get; set; }
        public string AssemblyId { get; set; }

        public int FindingQty { get; set; }
        public decimal FindingGmWtPerPc { get; set; }
        public decimal FindingTotalGm { get; set; }
        public decimal FindingCostPerPc { get; set; }
        public decimal FindingTotalCost { get; set; }
        public decimal FindingInc { get; set; }
        public decimal FindingDec { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}