using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class HastagPipointContext : IdentityDbContext<User>
    {
        public HastagPipointContext(DbContextOptions<HastagPipointContext> options)
            : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
    }
}
