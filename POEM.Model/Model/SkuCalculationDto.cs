using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    
    public class SkuCalculationDto
    {
        public decimal totalMetalCost { get; set; }
        public decimal totalFindingCost { get; set; }
        public decimal totalSemiStoneCost { get; set; }
        public decimal totalCenterStoneCost { get; set; }
        public decimal totalSemiSettingCost { get; set; }
        public decimal totalCenterSettingCost { get; set; }
        public decimal totalLaborCost { get; set; }
        public decimal semiDuty { get; set; }

        public decimal semiFOB { get; set; }
        public decimal completeFOB { get; set; }

        public decimal semiPrice1 { get; set; }
        public decimal semiPrice2 { get; set; }
        public decimal semiPrice3 { get; set; }
        public decimal semiPrice4 { get; set; }

        public decimal centerPrice1 { get; set; }
        public decimal centerPrice2 { get; set; }
        public decimal centerPrice3 { get; set; }
        public decimal centerPrice4 { get; set; }

        public decimal semiMargin1 { get; set; }
        public decimal semiMargin2 { get; set; }
        public decimal semiMargin3 { get; set; }
        public decimal semiMargin4 { get; set; }

        public decimal centerMargin1 { get; set; }
        public decimal centerMargin2 { get; set; }
        public decimal centerMargin3 { get; set; }
        public decimal centerMargin4 { get; set; }

        public decimal landedCost { get; set; }
        public decimal landedCostCenter { get; set; }
    }

}