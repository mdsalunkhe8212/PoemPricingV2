using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class SkuInfoDto
    {
        public VendorProductDto VendorProduct { get; set; }
        public List<MetalDto> Metals { get; set; }
        public List<FindingDto> Findings { get; set; }
    }
}