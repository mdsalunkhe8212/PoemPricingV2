using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class CategoryDetails
    {
        [Key]
        public long CategoryId { get; set; }
        public string Code { get; set; }

    
        public string CategoryName { get; set; }
    }
}