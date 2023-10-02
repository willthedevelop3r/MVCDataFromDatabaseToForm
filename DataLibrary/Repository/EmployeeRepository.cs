using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.Repository
{
    public class EmployeeRepository
    {

        private readonly SqlDataAccess _sqlDataAccess;

        public EmployeeRepository(SqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        /*   public int CreateEmployee(int employeeId, string firstName,
               string lastName, string emailAddress)
           {
               EmployeeModel data = new EmployeeModel
               {
                   EmployeeId = employeeId,
                   FirstName = firstName,
                   LastName = lastName,
                   EmailAddress = emailAddress

               };

               string sql = @"INSERT INTO dbo.Employee (EmployeeId, FirstName, LastName, EmailAddress)
                               VALUES (@EmployeeId, @FirstName, @LastName, @EmailAddress)";

               return _sqlDataAccess.SaveData(sql, data);
           }
   */
        public int CreateEmployee(EmployeeModel employee)
        {
            const string sql = @"INSERT INTO dbo.Employee (EmployeeId, FirstName, LastName, EmailAddress)
                         VALUES (@EmployeeId, @FirstName, @LastName, @EmailAddress)";

            return _sqlDataAccess.SaveData(sql, employee);
        }

        public int EditEmployee(EmployeeModel model, int originalEmployeeId)
        {
            // Fetching the employee details with the new ID (if it exists)
            var existingEmployee = GetEmployee(model.EmployeeId);

            // Check if the new ID is already taken by another record
            if (existingEmployee != null && existingEmployee.EmployeeId != originalEmployeeId)
            {
                throw new InvalidOperationException("Employee with this ID already exists");
            }

            string sql = @"UPDATE dbo.Employee 
                   SET 
                       EmployeeId = @EmployeeId,
                       FirstName = @FirstName, 
                       LastName = @LastName, 
                       EmailAddress = @EmailAddress 
                   WHERE EmployeeId = @OriginalEmployeeId";

            var parameters = new
            {
                EmployeeId = model.EmployeeId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                OriginalEmployeeId = originalEmployeeId
            };

            return _sqlDataAccess.SaveData(sql, parameters);
        }

        public List<EmployeeModel> LoadEmployees()
        {
            string sql = @"SELECT Id, EmployeeId, FirstName, LastName, EmailAddress
                            FROM dbo.Employee;";

            return _sqlDataAccess.LoadData<EmployeeModel>(sql);
        }

        public EmployeeModel GetEmployee(int id)
        {
            string sql = @"SELECT * FROM dbo.Employee WHERE EmployeeId = @Id";

            var param = new { Id = id };

            var result = _sqlDataAccess.LoadData<EmployeeModel>(sql, param).FirstOrDefault();

            return result;
        }

        public int DeleteEmployee(int id)
        {
            string sql = @"DELETE FROM dbo.Employee WHERE EmployeeId = @Id";

            var param = new { Id = id };

            return _sqlDataAccess.SaveData(sql, param);
        }
    }
}
