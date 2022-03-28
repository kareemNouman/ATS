using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;

namespace ATS.Service.Employees
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeViewModel> GetAllEmployee();

        bool AddEmployee(EmployeeViewModel employee);

        ATSGridResponseModel<EmployeeViewModel> GetAllEmployee(int skip, int take);

        Employee GetEmployee(Int64 id);
        EmployeeViewModel GetEmployeeByCode(long empCode);

        EmployeeViewModel GetEmployeeByID(Int64 id);      

        void UpdateEmployee(Employee employee);

        bool Delete(long ID);      
    }
}
