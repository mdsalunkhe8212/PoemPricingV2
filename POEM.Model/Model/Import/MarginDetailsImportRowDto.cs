using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class MarginDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }
        public string Vendor { get; set; }
        public string CategoryCode { get; set; }
        public string Category { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategory { get; set; }
        public string Metal { get; set; }
        public decimal PMargin1 { get; set; }
        public decimal PMargin2 { get; set; }
        public decimal PMargin3 { get; set; }
        public decimal PMargin4 { get; set; }
    }
}