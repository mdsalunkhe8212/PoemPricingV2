using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEM.Model.Model
{
    [Table("SKUStoneDetails")]
    public class SKUStoneDbDto
    {
        [Key]
        public long StoneId { get; set; }
        public long SKUId { get; set; }
        public string StoneVendor { get; set; }
        public string StoneVendorCode { get; set; }
        public string StoneType { get; set; }
        public string Growing { get; set; }
        public string SettingLocation { get; set; }
        public string Lab { get; set; }

        public string StoneShape { get; set; }
        public string ShapeText { get; set; }
        public string StoneMMSize { get; set; }
        public string StoneWidth1 { get; set; }
        public string StoneWidth2 { get; set; }

        public decimal PerStoneWt { get; set; }
        public int StoneQty { get; set; }
        public decimal TotalStoneWt { get; set; }
        public decimal TotalAdjStoneWt { get; set; }

        public string StoneQuality { get; set; }
        public decimal StoneCostPerCarat { get; set; }
        public decimal StoneTotalCost { get; set; }

        public string StoneSettingVendorCode { get; set; }
        public string StoneSettingVendor { get; set; }
        public string StoneSettingTypeCode { get; set; }
        public string StoneSettingType { get; set; }
        public decimal CostPerStone { get; set; }
        public decimal SettingTotalCost { get; set; }

        public decimal SemiMinWt { get; set; }
        public decimal SemiAdjWt { get; set; }
        public decimal CenterMinWt { get; set; }
        public decimal CenterAdjWt { get; set; }
        public decimal TotalMinWt { get; set; }
        public decimal TotalAdjWt { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}