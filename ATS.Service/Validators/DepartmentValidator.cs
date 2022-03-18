using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Service.Masters;
using FluentValidation;
using FluentValidation.Results;

namespace ATS.Service.Validators
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator(IMasterService masterService)
        {
            RuleSet("Add", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("DepartmentName Should Not Be Empty");
                Custom(p => masterService.GetDepartment(p.Name) != null ?
                            new ValidationFailure("Department Name", "Another DepartmentName with same name exists") :
                            null);
            });


            RuleSet("Update", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("DepartmentName Should Not Be Empty");
                Custom(p =>
                {
                    var department = masterService.GetDepartment(p.Name);

                    if (department != null ? department.ID != p.ID : false)
                        return new ValidationFailure("Department Name", "Another DepartmentName with same name exists");

                    return null;
                });
            });


        }
    }
}
