using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class SettingLaborDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }
        public string SettingVendor { get; set; }
        public string SettingType { get; set; }
        public string ShapeCode { get; set; }
        public string Shape { get; set; }
        public decimal? DiamondPSWtFrom { get; set; }
        public decimal? DiamondPSWtTo { get; set; }
        public decimal GoldCostPS { get; set; }

        public decimal PlatinumCostPS { get; set; }

        public decimal SilverCostPS { get; set; }
    }
}