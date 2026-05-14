using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class SettingLaborDetail
	{
        [Key]
        public long SettingLaborId { get; set; }
        public string Code { get; set; }
        public string SettingVendor { get; set; }
        public string ShapeCode { get; set; }
        public string Shape { get; set; }
        public decimal? DiamondPSWtFrom { get; set; }
        public decimal? DiamondPSWtTo { get; set; }

        public string DiamondPSWtRange { get; set; }

        public string SettingType { get; set; }

        public decimal GoldCostPS { get; set; }

        public decimal PlatinumCostPS { get; set; }

        public decimal SilverCostPS { get; set; }
        public string ExcludeShape { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        // Computed columns (no setter needed if EF maps as computed)
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal MinDiPSWtRange { get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal MaxDiPSWtRange { get; }

    }
}