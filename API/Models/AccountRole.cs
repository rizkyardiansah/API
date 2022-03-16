using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_t_account_role")]
    public class AccountRole
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string NIK { get; set; }
        
        [Required]
        public int RoleId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
