using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("SubCategoryMaster")]
    public class SubCategoryMasterDbDto
    {
        [Key]
        public int SubCategoryId { get; set; }

        public string Code { get; set; }
        public string SubCategoryName { get; set; }
    }
}