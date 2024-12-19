using EmpDeptSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace EmpDeptSys.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to Dashboard if logged in
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                // Redirect to Login page if not logged in
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // New Dashboard Action
        public IActionResult Dashboard()
        {
            return View(); // This will render the Dashboard.cshtml view in /Views/Home
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
