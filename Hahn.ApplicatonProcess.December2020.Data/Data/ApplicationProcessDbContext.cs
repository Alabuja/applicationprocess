using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicatonProcess.December2020.Data.Data
{
    public class ApplicationProcessDbContext : DbContext
    {
        public ApplicationProcessDbContext(DbContextOptions<ApplicationProcessDbContext> options)
            : base(options)
        {
            
        }

        public ApplicationProcessDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Applicant> Applicants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
