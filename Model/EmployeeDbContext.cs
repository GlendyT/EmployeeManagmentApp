using Microsoft.EntityFrameworkCore;

namespace Employee.api.Model
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)

        { }
        //public DbSet<Employee> Employees { get; set; }
    }
}