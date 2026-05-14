using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class MetalRateMasterDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        
        public string Metal { get; set; }

        [Required]
        public int MetalKT { get; set; }

        [Required]
      
        public decimal MetalRate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public int CreatedBy { get; set; } = 0;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}