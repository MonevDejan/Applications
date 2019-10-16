using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Invalid Name")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email@exaple.com")]
        public string Email { get; set; }
        public Dept? Department { get; set; }
        public string PhotoPath { get; set; }
    }
}
