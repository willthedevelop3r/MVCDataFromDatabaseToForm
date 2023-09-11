using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.BusinessLogic
{
    public class EmployeeProcessor
    {

        private readonly SqlDataAccess _sqlDataAccess;

        public EmployeeProcessor(SqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public int CreateEmployee(int employeeId, string firstName,
            string lastName, string emailAddress) 
        {
            EmployeeModel data = new EmployeeModel
            {
                EmployeeId = employeeId,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress

            };

            string sql = @"insert into dbo.Employee (EmployeeId, FirstName, LastName, EmailAddress)
                            values (@EmployeeId, @FirstName, @LastName, @EmailAddress)";

            return _sqlDataAccess.SaveData(sql, data);
        }

        public List<EmployeeModel> LoadEmployees()
        {
            string sql = @"select Id, EmployeeId, FirstName, LastName, EmailAddress
                            from dbo.Employee;";

            return _sqlDataAccess.LoadData<EmployeeModel>(sql);
        }
    }
}
