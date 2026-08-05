using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class DutyChartMasterRowDto : BaseImportRowDto
    {
        public string VendorLocation { get; set; }
        public decimal DutyPer { get; set; }
        public decimal TariffPer { get; set; }
        public decimal PenaltyPer { get; set; }
    }
}