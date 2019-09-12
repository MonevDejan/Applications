using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Marry1", Department = "HR1", Email = "marry1@gmail.com"},
                new Employee() { Id = 2, Name = "Marry2", Department = "HR2", Email = "marry2@gmail.com"},
                new Employee() { Id = 3, Name = "Marry3", Department = "HR3", Email = "marry3@gmail.com"}
            };
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }
    }
}
