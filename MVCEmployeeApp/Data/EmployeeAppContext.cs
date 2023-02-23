using Microsoft.EntityFrameworkCore;
using MVCEmployeeApp.Models.Domain;

namespace MVCEmployeeApp.Data
{
    public class EmployeeAppContext : DbContext
    {
        public EmployeeAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
