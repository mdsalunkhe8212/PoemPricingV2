using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class CollectionDtl
    {
        [Key]
        public long CollectionId { get; set; }
        public string Code { get; set; }

        public string Collection { get; set; }
    }
}