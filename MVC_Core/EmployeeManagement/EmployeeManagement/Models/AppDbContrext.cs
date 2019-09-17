using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class AppDbContrext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public AppDbContrext(DbContextOptions<AppDbContrext> options) : base(options)
        {

        }
    }
}
