using EmpDeptSys.Data;
using EmpDeptSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Employee_and_Department_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null) return NotFound();

            return View(employee);
        }


        // GET Create
        [HttpGet]
        public IActionResult Create()
        {
            // Fetch the departments from the database
            var departments = _context.Departments.ToList(); // Ensure _context is your DbContext instance
            if (departments != null && departments.Any())
            {
                ViewData["Departments"] = departments; // Pass the list of departments to the view
            }
            else
            {
                ViewData["Departments"] = new List<Department>(); // Handle cases where no departments exist
            }

            return View(); // Render the Create.cshtml view
        }


        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Log validation errors
                }
                return View(employee);
            }

            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); // Log exception message
                return View(employee); // Return the view with the employee to try again
            }

            return RedirectToAction(nameof(Index));
        }



        // GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Phone,Age,Image")] Employee employee)
        {
            if (id != employee.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET Delete 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
