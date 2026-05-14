using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class MarginDetailsDto
    {
        public long marginId { get; set; }

        public string vendor { get; set; }

        public string categoryCode { get; set; }

        public string category { get; set; }

        public string subCategoryCode { get; set; }

        public string subCategory { get; set; }

        public string metal { get; set; }

        public decimal? totalMinWt { get; set; }

        public decimal? pMargin1 { get; set; }

        public decimal? pMargin2 { get; set; }

        public decimal? pMargin3 { get; set; }

        public decimal? pMargin4 { get; set; }

        public int createdBy { get; set; }

        public DateTime createdOn { get; set; }

        public int? modifiedBy { get; set; }

        public DateTime? modifiedOn { get; set; }
    }
}