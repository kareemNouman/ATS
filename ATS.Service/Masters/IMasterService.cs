using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;

namespace ATS.Service.Masters
{
    public interface IMasterService
    {
        #region Departments
        IEnumerable<Department> GetAllDepartments();

        Department GetDepartment(Int64 id);

        long AddDepartment(Department department);

        long UpdateDepartment(Department department);

        Department GetDepartment(string name);

        #endregion

        #region Designations
        ATSGridResponseModel<UserAccountViewModel> GetAllUsers(int skip, int take);
        IEnumerable<Designation> GetAllDesignations();

        Designation GetDesignation(Int64 id);

        long AddDesignation(Designation department);

        long UpdateDesignation(Designation department);

        Designation GetDesignation(string name);

        #endregion

        #region Users

        IEnumerable<UserAccount> GetAllUsers();
        long AddAccount(UserAccountViewModel account);

        UserAccount GetAccount(Int64 id);

        long UpdateAccount(UserAccountViewModel account);

        bool DeleteAccount(long ID);
        #endregion

        #region Role
        IEnumerable<Role> GetAllRoles();

        Role GetRole(Int64 id);

        void AddRole(Role role);

        void UpdateRole(Role role);

        #endregion
    }
}
