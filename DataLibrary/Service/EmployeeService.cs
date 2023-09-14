using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.Service
{
    public class EmployeeProcessor
    {

        private readonly SqlDataAccess _sqlDataAccess;

        public EmployeeProcessor(SqlDataAccess sqlDataAccess)
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

        public int EditEmployee(EmployeeModel model)
        {
            string sql = @"UPDATE dbo.Employee 
                   SET 
                       FirstName = @FirstName, 
                       LastName = @LastName, 
                       EmailAddress = @EmailAddress 
                   WHERE EmployeeId = @EmployeeId";

            return _sqlDataAccess.SaveData(sql, model);
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
    }
}
