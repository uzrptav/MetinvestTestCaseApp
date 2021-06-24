using System;

namespace EF_Code_First
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://www.entityframeworktutorial.net/code-first/simple-code-first-example.aspx

            var context = new EmployeeContext();

            //using (var context = new EmployeeContext() )
            //{
                //for (int i = 1; i < 3; i++)
                //{
                //    var emp = new Employee 
                //    { 
                //        PersonnelID = i,
                //        EmployeeFullName = $"Employee_{i}",
                //        DateOfBirth = ( i >= 1 && i <= 12 ? DateTime.Now.AddYears(-20).AddMonths(i) : null  ),
                //        Gender = "male",
                //        IsStaffMember = true
                //    };

                //    context.Employees.Add(emp);
                //    context.SaveChanges();
                //}
            //}

            foreach (var emp in context.Employees)
            {
                Console.WriteLine($"{emp.PersonnelID}|{emp.EmployeeFullName}|{emp.DateOfBirth}|{emp.Gender}|{emp.IsStaffMember}");
            }
        }
    }
}
