using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("SKUDetails")]

    public class SKUDetailsDbDto
    {
        [Key]
        public long SKUId { get; set; }        
        public string Company { get; set; }
        public string SKUNumber { get; set; }
        public string Vendor { get; set; }
        public string VendorNumber { get; set; }
        public string OrderType { get; set; }
        public string CategoryCode { get; set; }
        public string Category { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategory { get; set; }
        public bool SKUStatus { get; set; }
        public string CollectionCode { get; set; }
        public string Collection { get; set; }
        public decimal SizeLength { get; set; }
        public decimal? MMWidth { get; set; }
        public decimal? MMHeight { get; set; }
        public bool IsActive { get; set; }                
        public decimal? semiMinWt { get; set; }        
        public decimal? centerMinWt { get; set; }        
        public decimal? SemiAdjWt { get; set; }        
        public decimal? CenterAdjWt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}