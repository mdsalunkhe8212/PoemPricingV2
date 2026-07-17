using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Report
{
    public class CollectionProposalDto
    {
        public long SKUId { get; set; }

        public string Company { get; set; }

        public string CompanyName { get; set; }

        public string SKUNumber { get; set; }

        public string Metal { get; set; }

        public decimal? SemiMount { get; set; }

        public decimal? SemiPrice1 { get; set; }

        public decimal? SemiPrice2 { get; set; }

        public decimal? SemiPrice3 { get; set; }

        public decimal? SemiPrice4 { get; set; }

        public string CenterShapeSize { get; set; }

        public decimal? CenterPrice1 { get; set; }

        public decimal? CenterPrice2 { get; set; }

        public decimal? CenterPrice3 { get; set; }

        public decimal? CenterPrice4 { get; set; }

        public decimal? TotalWeight { get; set; }

        public decimal? StoneTotalCost { get; set; }

        public string Findings { get; set; }

        public string CategoryCode { get; set; }

        public string Category { get; set; }

        public string Collection { get; set; }

        public string CollectionCode { get; set; }
    }
}