using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("DutyDetails")]
    public class DutyDetailsDbDto
    {

        [Key]
        public int DutyId { get; set; }


        public string VendorLocation { get; set; } = string.Empty;

       
        public bool Duty { get; set; }

       
        public bool Tariff { get; set; }

       
        public bool Penalty { get; set; }

       

        public string DiamondLocation { get; set; } = string.Empty;

       
        public bool DiamondDuty { get; set; }

       
        public bool DiamondTariff { get; set; }

       
        public bool DiamondPenalty { get; set; }


        public string LaborLocation { get; set; } = string.Empty;

       
        public bool LaborDuty { get; set; }

       
        public bool LaborTariff { get; set; }

       
        public bool LaborPenalty { get; set; }


        public string FindingLocation { get; set; } = string.Empty;

       
        public bool FindingDuty { get; set; }

       
        public bool FindingTariff { get; set; }

       
        public bool FindingPenalty { get; set; }

        // SQL type is BIT

        public long CreatedBy { get; set; }

       
        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}