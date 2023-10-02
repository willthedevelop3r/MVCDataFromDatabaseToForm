using DataLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVCApp.Models;
using System.Diagnostics;
namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmployeeRepository _employeeService;

        public HomeController(ILogger<HomeController> logger, EmployeeRepository employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // Get
        public IActionResult ViewEmployees() 
        {

            var data = _employeeService.LoadEmployees();
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

        // Post
        /*  [HttpPost]
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
          }*/

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(EmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                // Map employee model
                var dataLibraryModel = new DataLibrary.Models.EmployeeModel
                {
                    EmployeeId = model.EmployeeId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress
                };

                try
                {
                    int recordsCreated = _employeeService.CreateEmployee(dataLibraryModel);
                    _logger.LogInformation("Records created: {RecordsCreated}", recordsCreated);
                    return RedirectToAction("ViewEmployees");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation numbers
                    {
                        ModelState.AddModelError(string.Empty, "Employee ID already exists. Please use a different ID.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the data. Please try again later.");
                        _logger.LogError(ex, "An error occurred while creating a new employee.");
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var employee = _employeeService.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            var model = new EditEmployeeModel
            {
                OriginalEmployeeId = employee.EmployeeId,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailAddress = employee.EmailAddress
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEmployee(EditEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dataLibraryModel = new DataLibrary.Models.EmployeeModel
                    {
                        EmployeeId = model.EmployeeId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailAddress = model.EmailAddress
                    };
                    int recordsUpdated = _employeeService.EditEmployee(dataLibraryModel, model.OriginalEmployeeId);
                    _logger.LogInformation("Records updated: {RecordsUpdated}", recordsUpdated);
                    return RedirectToAction("ViewEmployees");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("EmployeeId", ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _employeeService.GetEmployee(id);
            if (employee == null)
            {
                return NotFound(); 
            }

            var model = new EditEmployeeModel
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailAddress = employee.EmailAddress
            };

            return View(model);
        }

        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmployeeConfirmed(int id)
        {
            _employeeService.DeleteEmployee(id);
            return RedirectToAction("ViewEmployees");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}