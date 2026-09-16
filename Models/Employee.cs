using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Practice.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set;  }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"\d{10}", ErrorMessage = "Enter a valid 10-digit Indian mobile number")]
        public string Phone { get; set; }

        [Required]
        [Range(10000, 10000000, ErrorMessage = "Salary must be between ₹10,000 and ₹1 crore.")]
        public decimal Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }
    }
}
