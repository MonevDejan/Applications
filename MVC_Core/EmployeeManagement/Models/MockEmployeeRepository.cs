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
                new Employee() { Id = 1, Name = "User1", Department = Dept.HR, Email = "User1@gmail.com"},
                new Employee() { Id = 2, Name = "User2", Department = Dept.IT, Email = "User2@gmail.com"},
                new Employee() { Id = 3, Name = "User3", Department = Dept.Payroll, Email = "User3@gmail.com"},
                new Employee() { Id = 3, Name = "User4", Department = Dept.Payroll, Email = "User4@gmail.com"},
                new Employee() { Id = 3, Name = "User5", Department = Dept.Payroll, Email = "User5@gmail.com"}
            };
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }
        public Employee Add(Employee employee)
        {
            employee.Id = (_employeeList.Max(e => e.Id) + 1);
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChange)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChange.Id);
            if (employee != null)
            {
                employee.Name = employeeChange.Name;
                employee.Email = employeeChange.Email;
                employee.Department= employeeChange.Department;
            }
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;

        }
    }
}
