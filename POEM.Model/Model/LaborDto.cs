using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class LaborDto
	{
        public string LaborLocation { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProcessType { get; set; }
        public decimal CastingLabor { get; set; }
        public int CastPcs { get; set; }
        public decimal CFP { get; set; }
        public decimal Rhodium { get; set; }
        public decimal LaborAssembly { get; set; }
        public decimal Solder { get; set; }
        public decimal Tag { get; set; }
        public decimal DiaHandling { get; set; }
        public decimal FinHandling { get; set; }
        public decimal Stamping { get; set; }
        public decimal Model { get; set; }
        public decimal CAM { get; set; }
        public decimal GiftBox { get; set; }
        public decimal TotalLabor { get; set; }

        public string OtherHead1 { get; set; }
        public decimal OtherCost1 { get; set; }
        public string OtherHead2 { get; set; }
        public decimal OtherCost2 { get; set; }
        public string OtherHead3 { get; set; }
        public decimal OtherCost3 { get; set; }

        public decimal SemiFOB { get; set; }
        public decimal SemiDuty { get; set; }
        public decimal LandedCost { get; set; }

        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }
        public decimal Price4 { get; set; }
        

        public decimal Margin1 { get; set; }
        public decimal Margin2 { get; set; }
        public decimal Margin3 { get; set; }
        public decimal Margin4 { get; set; }

        public decimal CompleteFOB { get; set; }
        public decimal CompleteDuty { get; set; }
        public decimal LandedCostComplete { get; set; }

        public decimal CompletePrice1 { get; set; }
        public decimal CompletePrice2 { get; set; }
        public decimal CompletePrice3 { get; set; }
        public decimal CompletePrice4 { get; set; }

        public decimal CompleteMargin1 { get; set; }
        public decimal CompleteMargin2 { get; set; }
        public decimal CompleteMargin3 { get; set; }
        public decimal CompleteMargin4 { get; set; }

        public string Remark { get; set; }

    }
}