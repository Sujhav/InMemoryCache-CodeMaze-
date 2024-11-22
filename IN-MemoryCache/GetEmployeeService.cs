using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;
using IN_MemoryCache;

namespace IN_MemoryCache
{
    public interface IGetEmployeeService
    {
        List<EmployeeModel> GetEmployee();
        ResponseDto.Enumdata AddEmployee(EmployeeModel employee);
    }

    public class GetEmployeeService : IGetEmployeeService
    {
        public List<EmployeeModel> GetEmployee()
        {
            List<EmployeeModel> employee = new();
            employee.Add(new EmployeeModel { FirstName = "Sujhav", LastName = "Maskey", Address = "Maitidevi" });
            employee.Add(new EmployeeModel { FirstName = "Sumit", LastName = "dhimal", Address = "kapan" });
            employee.Add(new EmployeeModel { FirstName = "saish", LastName = "manandhar", Address = "kalenki" });

            return employee;
        }

        public ResponseDto.Enumdata AddEmployee(EmployeeModel employee)
        {
            var data = GetEmployee();
            var count = data.Count();
            data.Add(new EmployeeModel()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
            });
            var newCount = data.Count();
            if (newCount > count)
            {
                return ResponseDto.Enumdata.success;
            }
            else
            {
                return ResponseDto.Enumdata.failure;
            }
        }
    }


}
