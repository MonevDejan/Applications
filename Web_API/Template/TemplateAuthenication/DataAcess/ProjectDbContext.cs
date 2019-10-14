using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAcess
{
    // Install NugetPackage: Microsoft.EntityFrameworkCore; 
    // Microsoft.EntityFrameworkCore.SqlServer; 
    // Microsoft.EntityFrameworkCore.Relational
    public class ProjectDbContext : IdentityDbContext<User>
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {

        }
    }
}
