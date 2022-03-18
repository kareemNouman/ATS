using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Service.Masters;
using ATS.Service.Messages;
using Syncfusion.JavaScript;

namespace ATS.Web.Controllers
{
    public class MasterController : BaseController
    {
        private readonly IMasterService _masterService;
        private readonly INotify _notify;
        public MasterController(IMasterService masterService, INotify notify)
        {
            _masterService = masterService;
            _notify = notify;
        }
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }

        #region Department
        //[NECAuthorize(Key = new string[] { NECPermissions.Department })]
        public ActionResult Departments()
        
        {
            var departments = _masterService.GetAllDepartments();
            List<ATS.Web.Models.DepartmentViewModel> departmentViewModel = new List<ATS.Web.Models.DepartmentViewModel>();
            foreach (var item in departments)
            {
                ATS.Web.Models.DepartmentViewModel model = new ATS.Web.Models.DepartmentViewModel();
                model.ID = item.ID;
                model.Name = item.Name;
                model.IsDelete = item.IsDelete;
                departmentViewModel.Add(model);
            }
            return View(departmentViewModel);
        }

        public ActionResult DepartmentAdd(DepartmentViewModel value)
        {           
            var department = new Department
            {
                Name = value.Name,
                IsDelete = false
            };
            var data = _masterService.AddDepartment(department);
            if (data != 0)
            {
                //var department = data;
                value.ID = department.ID;
                return Json(value, JsonRequestBehavior.AllowGet);
            }

            //Response.AddHeader("exception", "Error");

            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return Json(new ATSServiceResponse { IsSuccess = false, Errors = _notify.GetMessage() });
        }


        public ActionResult DepartmentUpdate(DepartmentViewModel value)
        {
            var department = new Department
            {
                ID= value.ID,
                Name = value.Name,
                IsDelete = false,
            };
            var data = _masterService.UpdateDepartment(department);
            if (data == 0)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return Json(new ATSServiceResponse { IsSuccess = false, Errors = _notify.GetMessage() });
            }
            return Json(data);
        }

        public ActionResult DepartmentDelete(Int64 key)
        {            
            var dbDepartment = _masterService.GetDepartment(key);
            dbDepartment.IsDelete = true;
            var data = _masterService.UpdateDepartment(dbDepartment);
            if (data == 0)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
            }
            return Json(dbDepartment, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Designations
        //[NECAuthorize(Key = new string[] { NECPermissions.Department })]
        public ActionResult Designations()

        {
            var departments = _masterService.GetAllDesignations();
            List<ATS.Core.Domain.DTO.DesignationViewModel> designationViewModel = new List<ATS.Core.Domain.DTO.DesignationViewModel>();
            foreach (var item in departments)
            {
                ATS.Core.Domain.DTO.DesignationViewModel model = new ATS.Core.Domain.DTO.DesignationViewModel();
                model.ID = item.ID;
                model.Name = item.Name;
                model.IsDelete = item.IsDelete;
                designationViewModel.Add(model);
            }
            return View(designationViewModel);
        }

        public ActionResult DesignationAdd(ATS.Core.Domain.DTO.DesignationViewModel value)
        {
            var designation = new Designation
            {
                Name = value.Name,
                IsDelete = false
            };
            var data = _masterService.AddDesignation(designation);
            if (data != 0)
            {
                value.ID = designation.ID;
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;           
            return Json(new ATSServiceResponse { IsSuccess =false, Errors = _notify.GetMessage() });
        }


        public ActionResult DesignationUpdate(ATS.Core.Domain.DTO.DesignationViewModel value)
        {
            var designation = new Designation
            {
                ID = value.ID,
                Name = value.Name,
                IsDelete = false,
            };
            var data = _masterService.UpdateDesignation(designation);
            if (data == 0)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;                
                return Json(new ATSServiceResponse { IsSuccess = false, Errors = _notify.GetMessage() });
            }
            return Json(data);
        }

        public ActionResult DesignationDelete(Int64 key)
        {
          
            var dbDesignation = _masterService.GetDesignation(key);
            dbDesignation.IsDelete = true;
            var data = _masterService.UpdateDesignation(dbDesignation);
            if (data == 0)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
            }
            return Json(dbDesignation, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Users

        public ActionResult Users()
        {
            return View();
        }

        // [NECAuthorize(Key = new string[] { NECPermissions.EmployeeCreate })]
        public ActionResult AddUsers()
        {
            UserAccountViewModel viewModel = new UserAccountViewModel();
            return View(viewModel);
        }
        [HttpPost]
        //  [NECAuthorize(Key = new string[] { NECPermissions.EmployeeCreate })]
        public ActionResult AddUsers(UserAccountViewModel viewmodel)
        {
            if (!ModelState.IsValid)
                return View(viewmodel);          

            var response = _masterService.AddAccount(viewmodel);
            if (response != 0)
            {
                SuccessNotification(Convert.ToString("User Added Successfully"));
                return RedirectToAction("Users");
            }
            else
            {
                ErrorNotification("Unable to Add User.Please Tyr again");
            }
            return View(viewmodel);
        }
        //[NECAuthorize(Key = new string[] { NECPermissions.EmployeeUpdate })]
        public ActionResult UserDetail(long id = 0)
        {
            ATS.Core.Domain.DTO.UserAccountViewModel viewmodel = new ATS.Core.Domain.DTO.UserAccountViewModel();           
            if (id > 0)
            {
                var user = _masterService.GetAccount(id);
                viewmodel.Id = user.ID;
                viewmodel.UserName = user.UserName;
                viewmodel.RoleID = user.RoleID;                
            }
            return View(viewmodel);
        }

        public ActionResult UserDelete(long id = 0)
        {
            var result = _masterService.DeleteAccount(id);
            if (result)
            {
                SuccessNotification("Successfully Deleted User");
            }
            else
                ErrorNotification("Unable to Delete User.Please Tyr again");
            return View("Users");
        }

        [HttpPost]
        //[NECAuthorize(Key = new string[] { NECPermissions.EmployeeUpdate })]
        public ActionResult UserDetail(UserAccountViewModel viewmodel)
        {
          var userID= _masterService.UpdateAccount(viewmodel);

            if (userID != 0)
            {                               
                SuccessNotification("User Successfully Updated");
            }
            else
            {
                ErrorNotification("Something Went Wrong!Please Try Again");
                return RedirectToAction("Index");
            }            
            return RedirectToAction("Users");
        }
        //    [NECAuthorize(Key = new string[] { NECPermissions.EmployeeRead })]
        public JsonResult UserData(DataManager dm)
        {
            if (dm.Search != null && dm.Search.Count > 0)
            {

            }
            var userdata = _masterService.GetAllUsers(dm.Skip, dm.Take);
            return Json(new { result = userdata.Data, count = userdata.Count }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetUserDetails(string userId)
        {
            UserAccountViewModel model = new UserAccountViewModel();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                model.Id = Convert.ToInt64(userId);
            }
            var usermodel = _masterService.GetAccount(model.Id);
            return Json(usermodel, JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}