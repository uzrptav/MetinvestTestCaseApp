using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace TestApp.DAL
{
    public class EmployeeDBInitializer: DropCreateDatabaseAlways<EmployeeDBContext>
    {
        protected override void Seed(EmployeeDBContext context)
        {
            IList<Employee> employees = new List<Employee>();

            //Populate Employee table
            for (int i = 1; i < 5; i++)
            {
                var emp = new Employee
                {
                    PersonnelID = Convert.ToString(i),
                    EmployeeFullName = $"Employee_{i}",
                    //DateOfBirth = Convert.ToString(DateTime.Now.AddYears(-20)),
                    DateOfBirth = DateTime.Now.AddYears(-20 - i),
                    Gender = "male",
                    IsStaffMember = true
                };

                employees.Add(emp);
            }
            context.Employees.AddRange(employees);

            base.Seed(context);
        }
    }
}