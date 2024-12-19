using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmpDeptSys.Models;
using Microsoft.AspNetCore.Identity;

namespace EmpDeptSys.Data
{
	public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}