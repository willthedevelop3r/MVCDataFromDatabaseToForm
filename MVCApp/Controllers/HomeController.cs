using DataLibrary.Service;
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

        // Get
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

                int recordsCreated = _employeeProcessor.CreateEmployee(dataLibraryModel);
            /*    _logger.LogInformation("Records updated: {RecordsCreated}", recordsCreated);
                Console.WriteLine(recordsCreated);*/
                return RedirectToAction("ViewEmployees");
            }
            
            return View();
        }


        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            // Retrieve the employee details using the id
            var employee = _employeeProcessor.GetEmployee(id);

            // Convert the data library model to your MVC model (if necessary)
            var model = new EditEmployeeModel
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailAddress = employee.EmailAddress
            };

            // Pass the model to the view
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEmployee(EditEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                // Map and update employee model (non-password properties)
                var dataLibraryModel = new DataLibrary.Models.EmployeeModel
                {
                    EmployeeId = model.EmployeeId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress
                };

                int recordsUpdated = _employeeProcessor.EditEmployee(dataLibraryModel);

                // Redirect to ViewEmployees if all validations passed
                return RedirectToAction("ViewEmployees");
            }

            // If ModelState is not valid, it means there are validation errors.
            // In this case, re-render the EditEmployee view with the same model
            // to display validation error messages.
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}