using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Data;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;
using FluentValidation;
using ATS.Service.Messages;

namespace ATS.Service.Employees
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IGenericRepository<Employee> _employeeRepository;        
        private readonly IUnitOfWork _unitOfWrk;

        private readonly IValidatorFactory _validatorFactory;
        private readonly INotify _notify;

        public EmployeeService(IGenericRepository<Employee> employeeRepository, 
            IUnitOfWork unitOfWrk,IValidatorFactory validatorFactory, INotify notify)
        {
            this._employeeRepository = employeeRepository;            
            this._unitOfWrk = unitOfWrk;

            _validatorFactory = validatorFactory;
            _notify = notify;
        }

        public IEnumerable<EmployeeViewModel> GetAllEmployee()
        {
            return _employeeRepository.GetWithInclude(x => x.IsActive == true).Select(x => new EmployeeViewModel
            {
                Id = x.ID,
                EmployeeCode =x.EmployeeCode,
                Name = x.Name,
                Email = x.Email,
                DepartmentID = x.DepartmentID.Value,
                DesignationID = x.DesignationID.Value,
                DORJ = x.DORJ
            });
        }

        public ATSGridResponseModel<EmployeeViewModel> GetAllEmployee(int skip, int take)
        {
            ATSGridResponseModel<EmployeeViewModel> res = new ATSGridResponseModel<EmployeeViewModel>();

            var employee = _employeeRepository.GetQueryable().Where(x => x.IsActive == true);

            if (employee != null)
            {
                res.Data = employee.Select(x => new EmployeeViewModel
                {
                    Id = x.ID,
                    EmployeeCode = x.EmployeeCode,
                    Name = x.Name,
                    Email = x.Email,
                    OTThreshold = x.OTThreshold,       
                    Gross = x.Gross,
                    IsOTEligible = x.IsOTEligible,
                    DORJ = x.DORJ             
                    //ShiftCode =x.ShiftCode
                }).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();

                res.Count = employee.Count();
            }
            return res;
        }
        public bool AddEmployee(EmployeeViewModel employeeviewmodel)
        {
            var result = false;
            var validator = _validatorFactory.GetValidator<EmployeeViewModel>();
            var res = validator.Validate(employeeviewmodel, ruleSet: "Add");

            if (!res.IsValid)
            {
                foreach (var item in res.Errors)
                    _notify.AddMessage(item.ErrorMessage);
                return false;
            }
            //bool IsEmail = CheckEmail(employeeviewmodel.Email);
            //if (IsEmail && employeeviewmodel.Email != null)
            //{
            //    _notify.AddMessage("Email Already Exist");
            //    return;

            //}

            
            var employee = new Employee
            {
                EmployeeCode = employeeviewmodel.EmployeeCode,
                Name = employeeviewmodel.Name,
                Email = employeeviewmodel.Email,
                DepartmentID = employeeviewmodel.DepartmentID,
                DesignationID= employeeviewmodel.DesignationID,
                Basic = employeeviewmodel.Basic,
                SplAllowance= employeeviewmodel.SplAllowance,
                Col = employeeviewmodel.Col,
                OthersAllowance= employeeviewmodel.OthersAllowance,
                Conveyance = employeeviewmodel.Conveyance,
                DORJ = employeeviewmodel.DORJ,
                Housing = employeeviewmodel.Housing,
                Gross= employeeviewmodel.Gross,
                OTThreshold = employeeviewmodel.OTThreshold,
                IsOTEligible = employeeviewmodel.IsOTEligible,
                WeekOffMain = employeeviewmodel.WeekOffMain,
                WeeklyOffAlternate = employeeviewmodel.WeeklyOffAlternate,
                CreatedBy = employeeviewmodel.CreatedBy,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _employeeRepository.Insert(employee);
            _unitOfWrk.Save();
            result = true;
            return result;
        }

        public Employee GetEmployee(long id)
        {
            return _employeeRepository.FirstOrDefault(x => x.ID == id && x.IsActive == true);
        }

        public void UpdateEmployee(Employee employee)
        {

            //var validator = _validatorFactory.GetValidator<Employee>();

            //var res = validator.Validate(employee, ruleSet: "Update");
            //if (!res.IsValid)
            //{
            //    foreach (var item in res.Errors)
            //        _notify.AddMessage(item.ErrorMessage);
            //    return;
            //}

            _employeeRepository.Update(employee);
            _unitOfWrk.Save();
        }

        public EmployeeViewModel GetEmployeeByID(long id)
        {

            return _employeeRepository.GetWithInclude(x => x.ID == id && x.IsActive == true).Select(x => new EmployeeViewModel
            {
                Id = x.ID,
                Name = x.Name,
                Email = x.Email,
                EmployeeCode = x.EmployeeCode,
                DesignationID = x.DesignationID,
                DORJ = x.DORJ,
                DepartmentID = x.DepartmentID,
                Basic = x.Basic,
                SplAllowance= x.SplAllowance,
                Col= x.Col,
                OthersAllowance= x.OthersAllowance,
                Conveyance = x.Conveyance.Value,
                Housing= x.Housing.Value,
                Gross = x.Gross.Value,
                OTThreshold = x.OTThreshold,
                WeekOffMain = x.WeekOffMain,
                ShiftCode = x.ShiftCode,
                WeeklyOffAlternate= x.WeeklyOffAlternate,
                IsOTEligible = x.IsOTEligible,
                IsActive = x.IsActive,
                CreatedBy= x.CreatedBy,
                CreatedOn = x.CreatedOn
            }).FirstOrDefault();
        }

        public bool Delete(long ID)
        {
            bool result = false;
            var employee = _employeeRepository.GetWithInclude(x => x.ID == ID && x.IsActive == true).FirstOrDefault();
            if (employee != null)
            {
                employee.IsActive = false;                
                _employeeRepository.Update(employee);
                _unitOfWrk.Save();
                result = true;

            }
            return result;
        }
    }
}
