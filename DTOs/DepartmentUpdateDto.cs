using System.ComponentModel.DataAnnotations;

namespace CRUD_Practice.Models
{
    public class DepartmentUpdateDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        public string DepartmentName { get; set; } = string.Empty;
    }
}