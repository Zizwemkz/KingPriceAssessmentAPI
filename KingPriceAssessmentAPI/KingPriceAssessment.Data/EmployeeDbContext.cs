using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeAllocation> EmployeeAllocation { get; set; }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure many-to-many relationship via the association class
            modelBuilder.Entity<EmployeeAllocation>()
                .HasOne(ea => ea.Role)
                .WithMany(e => e.EmployeeAllocation)
                .HasForeignKey(ea => ea.EmployeeId);

            modelBuilder.Entity<EmployeeAllocation>()
                .HasOne(ea => ea.Role)
                .WithMany(r => r.EmployeeAllocation)
                .HasForeignKey(ea => ea.RoleId);

            modelBuilder.Entity<EmployeeAllocation>()
                .HasOne(ea => ea.Department)
                .WithMany(d => d.EmployeeAllocation)
                .HasForeignKey(ea => ea.DepartmentId);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Excecutive" },
                new Role { Id = 2, RoleName = "Administrator" },
                new Role { Id = 3, RoleName = "Manager" },
                new Role { Id = 4, RoleName = "Supervisour" }, 
                new Role { Id = 5, RoleName = "Standard" }

            );

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, DepartmentName = "IT" },
                new Department { Id = 2, DepartmentName = "HR" },
                new Department { Id = 3, DepartmentName = "Finance" },
                new Department { Id = 4, DepartmentName = "Marketing" },
                new Department { Id = 5, DepartmentName = "Facility" }
            );
        }

    }
}
