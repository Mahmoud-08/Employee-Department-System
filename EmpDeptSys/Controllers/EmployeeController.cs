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
        public IActionResult Index()
        {
            var employees = _context.Employees.Include(e => e.Department).ToList();
            return View(employees);
        }


        // GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (employee == null) return NotFound();

            return View(employee);
        }


        // GET Create
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _context.Departments.ToList();
            ViewData["Departments"] = departments;
            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    employee.ImagePath = "/images/" + fileName;
                }

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Departments"] = _context.Departments.ToList();
            return View(employee);
        }

        // GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (employee == null) return NotFound();

            ViewBag.DepartmentId = new SelectList(await _context.Departments.ToListAsync(), "ID", "Name", employee.DepartmentId);
            return View(employee);
        }

        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee, IFormFile ImageFile)
        {
            if (id != employee.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    employee.ImagePath = $"/images/{fileName}";
                }

                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(await _context.Departments.ToListAsync(), "ID", "Name", employee.DepartmentId);
            return View(employee);
        }


        // GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.ID == id);

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

        // GET: Employee/Search - Search for employees based on the name
        public IActionResult Search(string searchQuery)
        {
            var employees = _context.Employees
                .Include(e => e.Department) // load the Department 
                .Where(e => e.Name.Contains(searchQuery)) // Filter employees by name
                .ToList();

            return PartialView("SearchedEmp", employees); // Return the partial view with filtered employees
        }

    }
}
