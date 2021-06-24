using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Code_First
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() : base()
        {
        }

        public DbSet<Employee> Employees { get; set;}
    }
}
