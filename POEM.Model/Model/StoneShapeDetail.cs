using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class StoneShapeDetail
	{
        [Key]
        public int StoneShapeId { get; set; }          // IDENTITY(1,1) 
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string StoneType { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string StoneShape { get; set; } = string.Empty;
        public string CategoryFancyRound { get; set; }
    }
}