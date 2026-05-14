using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class StoneDto
	{
        public string StoneVendor { get; set; }
        public string StoneVendorCode { get; set; }
        public string StoneType { get; set; }
        public string Growing { get; set; }
        public string SettingLocation { get; set; }
        public string Lab { get; set; }

        public string Shape { get; set; }
        public string ShapeText { get; set; }
        public string MMSize { get; set; }
        public string Width1 { get; set; }
        public string Width2 { get; set; }

        public string PerStoneWt { get; set; }
        public string Qty { get; set; }
        public string TotalStoneWt { get; set; }
        public string TotalAdjStoneWt { get; set; }

        public string StoneQuality { get; set; }
        public string StoneCostPerCarat { get; set; }
        public string StoneTotalCost { get; set; }

        public string SettingVendor { get; set; }
        public string SettingVendorCode { get; set; }
        public string SettingType { get; set; }
        public string SettingTypeCode { get; set; }
        public string CostPerStone { get; set; }
        public string TotalCost { get; set; }

        public string SemiMinWt { get; set; }
        public string SemiAdjWt { get; set; }
        public string CenterMinWt { get; set; }
        public string CenterAdjWt { get; set; }
        public string TotalMinWt { get; set; }
        public string TotalAdjWt { get; set; }

    }
}