using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class MetalLossDetails
	{
        [Key]
        public long MetalLossId { get; set; }              // bigint IDENTITY

        [Required, MaxLength(50)]
        public string VendorCode { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string VendorName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string MetalType { get; set; } = string.Empty;

        [Required]
        public int LossPer { get; set; }
    }
}