using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_education")]
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string GPA { get; set; }

        [Required]
        public int UniversityId { get; set; }

        public virtual ICollection<Profiling> Profilings { get; set; }
        public virtual University University { get; set; }
    }
}
