using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("SKUCalculations")]
    public class SkuCalculation
    {
        [Key]
        public long CalculationsId { get; set; }
        public long SKUId { get; set; }

        public decimal TotalMetalCost { get; set; }
        public decimal TotalFindingCost { get; set; }
        public decimal TotalSemiStoneCost { get; set; }
        public decimal TotalCenterStoneCost { get; set; }
        public decimal TotalSemiSettingCost { get; set; }
        public decimal TotalCenterSettingCost { get; set; }
        public decimal TotalLaborCost { get; set; }
        public decimal SemiDuty { get; set; }

        public decimal SemiFOB { get; set; }
        public decimal CompleteFOB { get; set; }

        public decimal SemiPrice1 { get; set; }
        public decimal SemiPrice2 { get; set; }
        public decimal SemiPrice3 { get; set; }
        public decimal SemiPrice4 { get; set; }

        public decimal CenterPrice1 { get; set; }
        public decimal CenterPrice2 { get; set; }
        public decimal CenterPrice3 { get; set; }
        public decimal CenterPrice4 { get; set; }

        public decimal SemiMargin1 { get; set; }
        public decimal SemiMargin2 { get; set; }
        public decimal SemiMargin3 { get; set; }
        public decimal SemiMargin4 { get; set; }

        public decimal CenterMargin1 { get; set; }
        public decimal CenterMargin2 { get; set; }
        public decimal CenterMargin3 { get; set; }
        public decimal CenterMargin4 { get; set; }

        public decimal LandedCost { get; set; }
        public decimal LandedCostCenter { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}