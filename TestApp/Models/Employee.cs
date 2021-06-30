using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestApp.Models.Validators;

namespace TestApp.DAL
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string PersonnelID { get; set; }
        public string EmployeeFullName { get; set; }
        public string Gender { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfBirth { get; set; }
        public bool IsStaffMember { get; set; }
    }
}
