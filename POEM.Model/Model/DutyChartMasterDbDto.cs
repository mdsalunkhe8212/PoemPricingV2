using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("DutyChartMaster")]
    public class DutyChartMasterDbDto
    {
        [Key]
        public long ID { get; set; }

       
        [StringLength(50)]
        public string VendorLocation { get; set; }

       
        public decimal DutyPer { get; set; }

       
        public decimal TariffPer { get; set; }

       
        public decimal PenaltyPer { get; set; }

       
        public long CreatedBy { get; set; }

       
        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

    
    }
}