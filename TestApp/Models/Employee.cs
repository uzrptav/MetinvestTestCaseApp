using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL
{
    public class Employee: IValidatableObject
    {
        private DateTime _dateOfBirth;
        public int EmployeeID { get; set; }
        public string PersonnelID { get; set; }
        public string EmployeeFullName { get; set; }
        public string Gender { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateOfBirth { 
            get 
            {
                return _dateOfBirth.Date; 
            }
            set 
            {
                if (value.HasValue)
                {
                    _dateOfBirth = value.Value;
                }               
            } 
        }
        public bool IsStaffMember { get; set; }

        public string GetValidateReport()
        {
            var validationResults = Validate(new ValidationContext(this));
            if (validationResults.Count() == 0)
                return String.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (var validationItem in validationResults)
            {
                sb.AppendLine($"{validationItem.MemberNames.First()} : {validationItem.ErrorMessage}");
            }

            return sb.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrEmpty(EmployeeFullName))
                yield return new ValidationResult("ФИО сотрудника не указано", new string[] { "EmployeeFullName" });

            if (IsStaffMember && string.IsNullOrEmpty(PersonnelID))
                yield return new ValidationResult("Табельный номер обязателен для сотрудников в штате", new string[] { "PersonnelID" });

            if (DateOfBirth.HasValue && (DateOfBirth >= DateTime.Now.AddYears(-18)))
            {
                yield return new ValidationResult("Сотрудник должен быть совершеннолетним", new string[] { "DateOfBirth" });
            }

            if (DateOfBirth.HasValue && (DateOfBirth <= DateTime.Now.AddYears(-60)))
            {
                yield return new ValidationResult("Сотрудник должен быть не старше 60 лет", new string[] { "DateOfBirth" });
            }
        }
    }
}
