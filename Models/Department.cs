using CRUD_Practice.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Practice.Models
{
    [Index(nameof(DepartmentName), IsUnique = true)]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}