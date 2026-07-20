using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class DutyDetailsImportRowDto : BaseImportRowDto
    {
        public string VendorLocation { get; set; }

        public bool Duty { get; set; }
        public bool Tariff { get; set; }
        public bool Penalty { get; set; }
        public string DiamondLocation { get; set; }
        public bool DiamondDuty { get; set; }
        public bool DiamondTariff { get; set; }
        public bool DiamondPenalty { get; set; }
        public string LaborLocation { get; set; }
        
        public bool LaborDuty { get; set; }
        public bool LaborTariff { get; set; }
        public bool LaborPenalty { get; set; }
        public string FindingLocation { get; set; }
        public bool FindingDuty { get; set; }
        public bool FindingTariff { get; set; }
        public bool FindingPenalty { get; set; }
    }
}