using System.ComponentModel.DataAnnotations;

namespace EmpDeptSys.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        public string Image { get; set; }

        // Foreign key to the Department table
        public int DepartmentId { get; set; }

        // Navigation property for the associated Department
        public Department Department { get; set; }
    }
}
