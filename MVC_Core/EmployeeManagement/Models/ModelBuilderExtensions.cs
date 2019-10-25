using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        
        public static void Seed(this ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Employee>().HasData(
                   new Employee() { Id = 1, Name = "User1", Department = Dept.HR, Email = "User1@gmail.com" },
                   new Employee() { Id = 2, Name = "User2", Department = Dept.IT, Email = "User2@gmail.com" },
                   new Employee() { Id = 3, Name = "User3", Department = Dept.Payroll, Email = "User3@gmail.com" },
                   new Employee() { Id = 4, Name = "User4", Department = Dept.Payroll, Email = "User4@gmail.com" },
                   new Employee() { Id = 5, Name = "User5", Department = Dept.Payroll, Email = "User5@gmail.com" }
            );

        }
    }

}
