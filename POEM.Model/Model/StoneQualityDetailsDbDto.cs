using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("StoneQualityDetails")]
    public class StoneQualityDetailsDbDto
    {
        [Key]
        public long StoneQualityId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [Required]
        [StringLength(50)]
        public string StoneVendorCode { get; set; }

        [Required]
        [StringLength(50)]
        public string StoneType { get; set; }

        [Required]
        [StringLength(10)]
        public string StoneShapeCode { get; set; }

        [Required]
        [StringLength(50)]
        public string StoneShape { get; set; }

        [Required]
        [StringLength(50)]
        public string StoneQuality { get; set; }

        [StringLength(50)]
        public string IntertionalGrading { get; set; }

    }
}