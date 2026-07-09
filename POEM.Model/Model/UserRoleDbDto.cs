using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    [Table("RoleDetails")]
    public class UserRoleDbDto
    {
        [Key]
        public int RoleId { get; set; }

        public string Role { get; set; }
    }
}