using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class MetalDto
	{
        public string metalText { get; set; }
        public string metalId { get; set; }
        public string karatText { get; set; }
        public string karatId { get; set; }
        public string colorText { get; set; }
        public string colorId { get; set; }

        public decimal gmWt { get; set; }
        public decimal ratePOz { get; set; }
        public decimal ratePerGm { get; set; }
        public decimal metalCost { get; set; }
        public string inc { get; set; }
        public string dec { get; set; }

    }
}