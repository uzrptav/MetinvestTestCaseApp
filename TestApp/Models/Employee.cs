using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.DAL
{
    public class Employee: IValidatableObject
    {
        DateTime _dateOfBirth;
        public int EmployeeID { get; set; }
        public string PersonnelID { get; set; }
        public string EmployeeFullName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public bool IsStaffMember { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {       

            if (IsStaffMember && string.IsNullOrEmpty(PersonnelID))
                yield return new ValidationResult("Табельный номер обязателен для сотрудников в штате");

            if (string.IsNullOrEmpty(DateOfBirth) && !DateTime.TryParse(DateOfBirth, out _dateOfBirth))
            {
                DateOfBirth = _dateOfBirth.ToShortDateString();
                yield return new ValidationResult("Неверный формат Даты рождения");
            }

            if (string.IsNullOrEmpty(PersonnelID) && !DateTime.TryParse(DateOfBirth, out _dateOfBirth) && (_dateOfBirth >= _dateOfBirth.AddYears(-18) ))
            {
                yield return new ValidationResult("Сотрудник должен быть совершеннолетним");
            }

        }
    }
}
