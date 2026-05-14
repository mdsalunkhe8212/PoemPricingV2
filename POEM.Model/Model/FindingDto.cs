using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class FindingDto
	{
        public string supplier { get; set; }
        public string supplierId { get; set; }
        public string type { get; set; }
        public string typeId { get; set; }
        public string description { get; set; }
        public string sku { get; set; }

        public string metal { get; set; }
        public string karat { get; set; }
        public string color { get; set; }

        public string assembly { get; set; }
        public string assemblyId { get; set; }

        public int qty { get; set; }
        public decimal gmWtPerPc { get; set; }
        public decimal totalGm { get; set; }
        public decimal costPerPc { get; set; }
        public decimal totalCost { get; set; }
        public string inc { get; set; }
        public string dec { get; set; }

    }
}