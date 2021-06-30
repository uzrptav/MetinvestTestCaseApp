using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using TestApp.DAL;
using ModelValidatorProvider = System.Web.Mvc.ModelValidatorProvider;
using ModelValidator = System.Web.Mvc.ModelValidator;
using ModelMetadata = System.Web.Mvc.ModelMetadata;

namespace TestApp.Models.Validators
{
    public class EmployeeModelValidatorProvider : ModelValidatorProvider
    {

        public override IEnumerable<System.Web.Mvc.ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            if (metadata.ModelType == typeof(Employee))
                yield return new EmployeeModelValidator(metadata, context);
        }
    }
}