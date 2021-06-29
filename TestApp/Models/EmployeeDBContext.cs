using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Models.Exceptions;

namespace TestApp.DAL
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext() : base()
        {
            //Set EmployeeDBInitializer initializer class in context class
            Database.SetInitializer(new EmployeeDBInitializer());
        }

        public DbSet<Employee> Employees { get; set;}

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
          
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                throw newException;
            }
            catch (DbUpdateException e)
            {
                var newException = new FormattedDbUpdateException(e);
                throw newException;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}
