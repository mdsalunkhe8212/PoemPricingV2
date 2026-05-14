using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class DiamondDetail
	{
        [Key]
        public long DiamondId { get; set; }                // bigint IDENTITY

        [Required, MaxLength(50)]
        public string VendorCode { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string StoneType { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string GrowingType { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string StoneShape { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string StoneQuality { get; set; } = string.Empty;

        [MaxLength(50)]
        public string SizeRange { get; set; }

        [MaxLength(50)]
        public string SieveSize { get; set; }
        
        public decimal LengthDiameter { get; set; }

        public decimal? Width1 { get; set; }

        public decimal? Width2 { get; set; }

        public decimal PerStoneWeight { get; set; }

        public string StoneCertificate { get; set; }

        public decimal? CostPerCt { get; set; }
        
    }
}