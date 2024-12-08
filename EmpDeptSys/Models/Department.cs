using System.ComponentModel.DataAnnotations;
namespace EmpDeptSys.Models
{
    public class Department
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Range(1, 100)]
        public int Age { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>(); 
    }
}
