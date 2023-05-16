using Dallas.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dallas.Core.Data
{
    public class MainDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();

        public MainDbContext(DbContextOptions options)
            : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(b =>
            {
                b.HasKey(x => x.EmployeeID);
                                
                b.HasIndex(x => x.Name);
                b.Property(x => x.Name).HasMaxLength(256);
                
                b.HasIndex(x => x.JobTitle);
                b.Property(x => x.JobTitle).HasMaxLength(512);
            });
        }
    }
}
