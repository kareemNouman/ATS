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
    public class PublicHolidaysValidator : AbstractValidator<PublicHolidays>
    {
        public PublicHolidaysValidator(IMasterService masterService)
        {
            RuleSet("Add", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("PublicHoliday Name Should Not Be Empty");
                RuleFor(p => p.Date).NotEmpty().WithMessage("PublicHoliday Date Should Not Be Empty");
                Custom(p => masterService.GetDepartment(p.Name) != null ?
                            new ValidationFailure("PublicHoliday Name", "Another PublicHoliday with same name exists") :
                            null);
            });


            RuleSet("Update", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("PublicHolidayName Should Not Be Empty");
                RuleFor(p => p.Date).NotEmpty().WithMessage("PublicHoliday Date Should Not Be Empty");
                Custom(p =>
                {
                    var department = masterService.GetDepartment(p.Name);

                    if (department != null ? department.ID != p.ID : false)
                        return new ValidationFailure("PublicHoliday Name", "Another PublicHoliday with same name exists");

                    return null;
                });
            });


        }
    }
}
