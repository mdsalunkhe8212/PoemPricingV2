using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("MarginDetails")]
    public class MarginDetailsDbDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MarginId { get; set; }

        public string Vendor { get; set; }

        public string CategoryCode { get; set; }

        public string Category { get; set; }

        public string SubCategoryCode { get; set; }

        public string SubCategory { get; set; }

        public string Metal { get; set; }

     
        public decimal? TotalMinWt { get; set; }

        
        public decimal? PMargin1 { get; set; }

        
        public decimal? PMargin2 { get; set; }

        
        public decimal? PMargin3 { get; set; }

        
        public decimal? PMargin4 { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}