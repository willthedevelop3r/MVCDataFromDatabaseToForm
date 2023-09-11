using DataLibrary.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;
using System.Diagnostics;
namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmployeeProcessor _employeeProcessor;

        public HomeController(ILogger<HomeController> logger, EmployeeProcessor employeeProcessor)
        {
            _logger = logger;
            _employeeProcessor = employeeProcessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewEmployees() 
        {

            var data = _employeeProcessor.LoadEmployees();
            List<EmployeeModel> employees = new List<EmployeeModel>();

            foreach (var row in data) 
            {
                employees.Add(new EmployeeModel
                {
                    EmployeeId = row.EmployeeId,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    EmailAddress = row.EmailAddress,
                    ConfirmEmail = row.EmailAddress
                });
            }
            

            return View(employees);
        }

        // Get
        public IActionResult SignUp()
        {
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(EmployeeModel model)
        {
            if (ModelState.IsValid) 
            {
                int recordsCreated = _employeeProcessor.CreateEmployee(model.EmployeeId, 
                    model.FirstName, 
                    model.LastName, 
                    model.EmailAddress);
                return RedirectToAction("ViewEmployees");    
            }

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}