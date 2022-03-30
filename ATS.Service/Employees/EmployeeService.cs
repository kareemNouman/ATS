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
        private readonly IGenericRepository<Designation> _designationRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IUnitOfWork _unitOfWrk;

        private readonly IValidatorFactory _validatorFactory;
        private readonly INotify _notify;

        public EmployeeService(IGenericRepository<Employee> employeeRepository, IGenericRepository<Designation> designationRepository,
            IGenericRepository<Department> departmentRepository,
            IUnitOfWork unitOfWrk,IValidatorFactory validatorFactory, INotify notify)
        {
            this._employeeRepository = employeeRepository;            
            this._unitOfWrk = unitOfWrk;
            this._designationRepository = designationRepository;
            this._departmentRepository = departmentRepository;
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

        public ATSGridResponseModel<EmployeeViewModel> GetAllEmployee(int skip, int take,string empName)
        {
            ATSGridResponseModel<EmployeeViewModel> res = new ATSGridResponseModel<EmployeeViewModel>();
            IQueryable<Employee> employees = _employeeRepository.GetQueryable().Where(x => x.IsActive == true); ;
            if (!string.IsNullOrEmpty(empName))            
               employees = employees.Where(x => x.Name.Contains(empName));

            if (employees != null)
            {
                res.Data = employees.Select(x => new EmployeeViewModel
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

                res.Count = employees.Count();
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
                ShiftCode = employeeviewmodel.ShiftCode,
                IsOTEligible = employeeviewmodel.IsOTEligible,
                WeekOffMain = employeeviewmodel.WeekOffMain,
                WeeklyOffAlternate = employeeviewmodel.WeeklyOffAlternate != null ? employeeviewmodel.WeeklyOffAlternate.Value: 0,
                CreatedBy = 1,
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
                ShiftCode = x.ShiftCode.Value,
                WeeklyOffAlternate= x.WeeklyOffAlternate,
                IsOTEligible = x.IsOTEligible,
                IsActive = x.IsActive,
                CreatedBy= x.CreatedBy,
                CreatedOn = x.CreatedOn
            }).FirstOrDefault();
        }

        public EmployeeViewModel GetEmployeeByCode(long empCode)
        {
            EmployeeViewModel viewModel = new EmployeeViewModel();
            var emp = _employeeRepository.GetWithInclude(x => x.EmployeeCode == empCode && x.IsActive == true).FirstOrDefault();
            if (emp != null)
            {
            viewModel = new EmployeeViewModel {
                Id = emp.ID,
                Name = emp.Name,
                Email = emp.Email,
                EmployeeCode = emp.EmployeeCode,
                DesignationID = emp.DesignationID,
                DepartmentID = emp.DepartmentID,
                Designation = emp.DesignationID != null ? _designationRepository.GetByID(emp.DesignationID).Name: string.Empty,
                Department = emp.DepartmentID != null ? _departmentRepository.GetByID(emp.DepartmentID).Name : string.Empty,
                DORJ = emp.DORJ,
                Basic = emp.Basic,
                SplAllowance = emp.SplAllowance,
                Col = emp.Col,
                OthersAllowance = emp.OthersAllowance,
                Conveyance = emp.Conveyance.Value,
                Housing = emp.Housing.Value,
                Gross = emp.Gross.Value,
                OTThreshold = emp.OTThreshold,
                WeekOffMain = emp.WeekOffMain,
                ShiftCode = emp.ShiftCode.Value,
                WeeklyOffAlternate = emp.WeeklyOffAlternate,
                IsOTEligible = emp.IsOTEligible,
                IsActive = emp.IsActive,
                CreatedBy = emp.CreatedBy,
                CreatedOn = emp.CreatedOn
            };
            }
            return viewModel;
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
