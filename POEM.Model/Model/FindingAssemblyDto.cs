using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class FindingAssemblyDto
	{
        [Key]
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
    }
}