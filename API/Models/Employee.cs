using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_employee")]
    public class Employee
    {
        [Key]
        public string NIK { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int Salary { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public virtual Account Account { get; set; }
    }

    public enum Gender {
        Male,
        Female
    }
}
