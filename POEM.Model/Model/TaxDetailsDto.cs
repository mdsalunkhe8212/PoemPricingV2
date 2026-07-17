using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class TaxDetailsDto
    {
        public string VendorLocation { get; set; } = string.Empty;


        public bool Duty { get; set; }


        public bool Tariff { get; set; }


        public bool Penalty { get; set; }
        public decimal DutyPer { get; set; }


        public decimal TariffPer { get; set; }


        public decimal PenaltyPer { get; set; }

    }
}