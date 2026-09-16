using CRUD_Practice.Models;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Practice.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; } = string.Empty;
    }
}