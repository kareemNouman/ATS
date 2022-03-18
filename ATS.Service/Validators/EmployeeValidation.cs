using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DTO;
using ATS.Service.Employees;
using FluentValidation;
using FluentValidation.Results;

namespace ATS.Service.Validators
{
    public class EmployeeValidation : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeValidation(IEmployeeService employeeService)
        {
            RuleSet("Add", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("Name field is requried");
                RuleFor(p => p.DepartmentID).NotEmpty().WithMessage("Select Department");
                RuleFor(p => p.DesignationID).NotEmpty().WithMessage("Select Designation");
                RuleFor(p => p.EmployeeCode).NotEmpty().WithMessage("EmployeeCode is required");
            });

            RuleSet("Update", () =>
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("Name field is requried");
                RuleFor(p => p.DepartmentID).NotEmpty().WithMessage("Select Department");
                RuleFor(p => p.DesignationID).NotEmpty().WithMessage("Select Designation");
                RuleFor(p => p.EmployeeCode).NotEmpty().WithMessage("EmployeeCode is required");
            });

        }
    }
}
