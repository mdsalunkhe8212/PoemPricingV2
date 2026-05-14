using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class SkuModuleDto
    {
        public SkuInfoDto skuInfo { get; set; }
        public List<StoneDto> stoneInfo { get; set; }
        public LaborDto laborInfo { get; set; }
        public SkuCalculationDto calculations { get; set; }
    }
}