using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.api.Model
{
    [Table("designationTbl")]
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int designationId { get; set; }
        [Required]
        public int departmentId { get; set; }
        [Required, MaxLength(50)]
        public string designationName { get; set; } = string.Empty;


    }
}