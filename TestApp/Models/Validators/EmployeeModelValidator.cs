using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestApp.DAL;

namespace TestApp.Models.Validators
{
    public class EmployeeModelValidator : ModelValidator
    {
        public EmployeeModelValidator(ModelMetadata metadata, ControllerContext context): base(metadata, context) { }
  

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var model = (Employee)Metadata.Model;

            if (string.IsNullOrEmpty(model.EmployeeFullName))
                yield return new ModelValidationResult
                {
                    MemberName  = "EmployeeFullName",
                    Message     = "ФИО сотрудника не указано"
                };

            if (model.IsStaffMember)
            {
                if (string.IsNullOrEmpty(model.PersonnelID)) 
                    yield return new ModelValidationResult
                    {
                        MemberName = "IsStaffMember",
                        Message = "Табельный номер обязателен для сотрудников в штате"
                    };

            }
            if (model.DateOfBirth.HasValue && (model.DateOfBirth >= DateTime.Now.AddYears(-18)))
                yield return new ModelValidationResult
                {
                    MemberName  = "DateOfBirth",
                    Message     = "Сотрудник должен быть совершеннолетним"
                };


            if (model.DateOfBirth.HasValue && (model.DateOfBirth <= DateTime.Now.AddYears(-60)))
                yield return new ModelValidationResult
                {
                    MemberName  = "DateOfBirth",
                    Message     = "Сотрудник должен быть не старше 60 лет"
                };
        }
    }
}