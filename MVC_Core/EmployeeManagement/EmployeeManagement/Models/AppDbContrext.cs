using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class AppDbContrext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employee> Employees { get; set; }
        public AppDbContrext(DbContextOptions<AppDbContrext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                   new Employee() { Id = 1, Name = "Marry1", Department = Dept.HR, Email = "marry1@gmail.com" },
                   new Employee() { Id = 2, Name = "Marry2", Department = Dept.IT, Email = "marry2@gmail.com" },
                   new Employee() { Id = 3, Name = "Marry3", Department = Dept.Payroll, Email = "marry3@gmail.com" }
                   );

            base.OnModelCreating(modelBuilder);

            

            
        }
    }
}
