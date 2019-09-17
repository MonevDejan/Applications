﻿using System;
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
                new Employee() { Id = 0, Name = "Marry1", Department = Dept.HR, Email = "marry1@gmail.com"},
                new Employee() { Id = 1, Name = "Marry2", Department = Dept.IT, Email = "marry2@gmail.com"},
                new Employee() { Id = 2, Name = "Marry3", Department = Dept.Payroll, Email = "marry3@gmail.com"}
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
    }
}
