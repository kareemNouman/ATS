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
    public class LeavesValidator: AbstractValidator<Leaves>
    {
        public LeavesValidator(IMasterService masterService)
        {
            RuleSet("Add", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("LeaveTypeName Should Not Be Empty");
                Custom(p => masterService.GetLeaves(p.Name) != null ?
                            new ValidationFailure("LeaveType Name", "Another LeaveType with same name exists") :
                            null);
            });


            RuleSet("Update", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("LeaveType Should Not Be Empty");
                Custom(p =>
                {
                    var department = masterService.GetLeaves(p.Name);

                    if (department != null ? department.ID != p.ID : false)
                        return new ValidationFailure("LeaveType Name", "Another LeaveType with same name exists");

                    return null;
                });
            });

        }
    }
}
