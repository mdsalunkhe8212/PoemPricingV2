using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("CompanyMaster")]
    public class CompanyMasterDbDto
    {
        [Key]
        public long ID { get; set; }

      
        public string Code { get; set; }

        public string CompanyName { get; set; }
    }
}