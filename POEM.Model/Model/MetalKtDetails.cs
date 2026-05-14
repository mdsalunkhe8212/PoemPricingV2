using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class MetalKtDetails
	{
        [Key]
        public long MetalKtId { get; set; }
        public string MetalType { get; set; }
        public int MetalKt { get; set; }
        public decimal MetalRate { get; set; }        
    }
}