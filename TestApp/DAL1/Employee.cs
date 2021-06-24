using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public int PersonnelID { get; set; }
        public string EmployeeFullName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsStaffMember { get; set; }
    }
}
