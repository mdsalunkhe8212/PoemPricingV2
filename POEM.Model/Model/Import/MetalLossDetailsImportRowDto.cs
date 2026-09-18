using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class MetalLossDetailsImportRowDto : BaseImportRowDto
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string MetalType { get; set; }

        public decimal LossPer { get; set; }
    }
}