using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;
using ATS.Data;
using ATS.Service.Messages;
using ATS.Service.Security;
using FluentValidation;

namespace ATS.Service.Masters
{
    public class MasterService:IMasterService
    {
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Designation> _designationRepository;
        private readonly IGenericRepository<Role> _roleRepository;      
        private readonly IGenericRepository<UserAccount> _accountRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWrk;


        private readonly IValidatorFactory _validatorFactory;
        private readonly INotify _notify;

        public MasterService(IGenericRepository<Department> departmentRepository, 
            IGenericRepository<Designation> designationRepository,
            IGenericRepository<Role> roleRepository, IGenericRepository<UserAccount> accountRepository, 
            IEncryptionService encryptionService,
            IUnitOfWork unitOfWrk, IValidatorFactory validatorFactory, INotify notify)
        {

            this._departmentRepository = departmentRepository;
            this._designationRepository = designationRepository;
            this._roleRepository = roleRepository;            
            this._accountRepository = accountRepository;
            this._encryptionService = encryptionService;
            this._unitOfWrk = unitOfWrk;

            _validatorFactory = validatorFactory;
            _notify = notify;
        }


        #region Department

        public IEnumerable<Department> GetAllDepartments()
        {
            return _departmentRepository.GetAll().Where(x => x.IsDelete == false);
        }

        public Department GetDepartment(Int64 id)
        {
            return _departmentRepository.FirstOrDefault(x => x.ID == id);
        }

        public Department GetDepartment(string name)
        {
            return _departmentRepository.GetQueryable().FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.IsDelete == false);
        }
        public long AddDepartment(Department department)
        {            
            var validator = _validatorFactory.GetValidator<Department>();
            //validator.ValidateAndThrow(state);

            var res = validator.Validate(department, ruleSet: "Add");
            if (!res.IsValid)
            {
                foreach (var item in res.Errors)
                    _notify.AddMessage(item.ErrorMessage);
                return 0;
            }

            _departmentRepository.Insert(department);
            _unitOfWrk.Save();

            return department.ID;
        }

        public long UpdateDepartment(Department department)
        {
            var validator = _validatorFactory.GetValidator<Department>();

            var res = validator.Validate(department, ruleSet: "Update");
            if (!res.IsValid)
            {
                foreach (var item in res.Errors)
                    _notify.AddMessage(item.ErrorMessage);
                return 0;
            }
            _departmentRepository.Update(department);
            _unitOfWrk.Save();
            return department.ID;
        }

        #endregion

        #region Designations

        public IEnumerable<Designation> GetAllDesignations()
        {
            return _designationRepository.GetAll().Where(x => x.IsDelete == false);
        }

        public Designation GetDesignation(Int64 id)
        {
            return _designationRepository.FirstOrDefault(x => x.ID == id);
        }

        public Designation GetDesignation(string name)
        {
            var designation = _designationRepository.GetQueryable().FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.IsDelete == false);
            return designation;
        }
        public long AddDesignation(Designation designation)
        {
            var validator = _validatorFactory.GetValidator<Designation>();
            //validator.ValidateAndThrow(state);

            var res = validator.Validate(designation, ruleSet: "Add");
            if (!res.IsValid)
            {
                foreach (var item in res.Errors)
                    _notify.AddMessage(item.ErrorMessage);
                return 0;
            }

            _designationRepository.Insert(designation);
            _unitOfWrk.Save();

            return designation.ID;
        }

        public long UpdateDesignation(Designation designation)
        {
            var validator = _validatorFactory.GetValidator<Designation>();

            var res = validator.Validate(designation, ruleSet: "Update");
            if (!res.IsValid)
            {
                foreach (var item in res.Errors)
                    _notify.AddMessage(item.ErrorMessage);
                return 0;
            }
            _designationRepository.Update(designation);
            _unitOfWrk.Save();
            return designation.ID;
        }

        #endregion

        #region userAccount

        public IEnumerable<UserAccount> GetAllUsers()
        {
            return _accountRepository.GetAll().Where(x => x.IsActive == false);
        }

        public ATSGridResponseModel<UserAccountViewModel> GetAllUsers(int skip, int take)
        {
            ATSGridResponseModel<UserAccountViewModel> res = new ATSGridResponseModel<UserAccountViewModel>();

            var user = _accountRepository.GetQueryable().Where(x => x.IsActive == true);

            if (user != null)
            {
                res.Data = user.Select(x => new UserAccountViewModel
                {
                    Id = x.ID,
                    UserName = x.UserName,
                    RoleID = x.RoleID
                    //Password= _encryptionService.DecryptText,
                    
                }).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();

                res.Count = user.Count();
            }
            return res;
        }
        public long AddAccount(UserAccountViewModel account)
        {
            string saltKey = _encryptionService.CreateSaltKey(5);
            var pwdHash = _encryptionService.CreatePasswordHash(account.Password, saltKey);

            var user = new UserAccount
            {
                UserName = account.UserName,
                PasswordHash = pwdHash,
                Salt = saltKey,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                IsActive = true,
                RoleID = account.RoleID                
            };
            _accountRepository.Insert(user);
            _unitOfWrk.Save();
            return user.ID;
        }

        public UserAccount GetAccount(Int64 id)
        {
            return _accountRepository.FirstOrDefault(x => x.ID == id);
        }

        public long UpdateAccount(UserAccountViewModel account)
        {
            string saltKey = _encryptionService.CreateSaltKey(5);
            var pwdHash = _encryptionService.CreatePasswordHash(account.Password, saltKey);

            var user = new UserAccount
            {
                UserName = account.UserName,
                PasswordHash = pwdHash,
                Salt = saltKey,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                IsActive = true,
                RoleID = account.RoleID
            };
           
            _accountRepository.Update(user);
            _unitOfWrk.Save();
            return user.ID;
        }

        public bool DeleteAccount(long ID)
        {
            bool result = false;
            var user = _accountRepository.GetWithInclude(x => x.ID == ID && x.IsActive == true).FirstOrDefault();
            if (user != null)
            {
                user.IsActive = false;
                _accountRepository.Update(user);
                _unitOfWrk.Save();
                result = true;

            }
            return result;
        }

        #endregion

        #region Role
        public IEnumerable<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }

        public Role GetRole(Int64 id)
        {
            return _roleRepository.FirstOrDefault(x => x.ID == id);
        }

        public void AddRole(Role role)
        {
            _roleRepository.Insert(role);
            _unitOfWrk.Save();
        }
        public void UpdateRole(Role role)
        {
            _roleRepository.Update(role);
            _unitOfWrk.Save();
        }
        #endregion
    }
}
