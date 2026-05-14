using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class MetelColorDtls
	{
        [Key]
        public long MetelColorId { get; set; }
        public string MetalType { get; set; }
        public string MetelColor { get; set; }
    }
}