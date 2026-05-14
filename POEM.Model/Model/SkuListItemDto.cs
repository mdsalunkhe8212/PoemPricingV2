using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class SkuListItemDto
    {
        public string Sku { get; set; }
        public string Top1Metal { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } // derived from IsActive = 1
    }

    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Items { get; set; }
    }
}