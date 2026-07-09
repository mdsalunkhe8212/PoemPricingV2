using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class DiamondDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }
        public string VendorCode  { get; set; }
        public string StoneType { get; set; }
        public string GrowingType { get; set; }
        public string StoneShapeCode { get; set; }
        public string StoneShape { get; set; }
        public string StoneQualityCode { get; set; }
        public string StoneQuality { get; set; }
        public string SizeRange { get; set; }
        public decimal SizeFrom { get; set; }
        public decimal SizeTo { get; set; }
        public string SieveSize { get; set; }
        public decimal LengthDiameter { get; set; }
        public decimal Width1 { get; set; }
        public decimal Width2 { get; set; }
        public decimal PerStoneWeight { get; set; }
        public string StoneCertificate { get; set; }
        public decimal CostPerCt { get; set; }
    }
}