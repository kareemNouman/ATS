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
    public class DesignationValidator : AbstractValidator<Designation>
    {
        public DesignationValidator(IMasterService masterService)
        {
            RuleSet("Add", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("DesignationName Should Not Be Empty");
                Custom(p => masterService.GetDesignation(p.Name) != null ?
                            new ValidationFailure("Designation Name", "Another Designation with same name exists") :
                            null);
            });


            RuleSet("Update", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("DesignationName Should Not Be Empty");
                Custom(p =>
                {
                    var department = masterService.GetDesignation(p.Name);

                    if (department != null ? department.ID == p.ID : false)
                        return new ValidationFailure("Designation Name", "Another DesignationName with same name exists");

                    return null;
                });
            });


        }
    }
}
