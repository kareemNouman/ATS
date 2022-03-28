using ATS.Core.Common;
using ATS.Service.Employees;
using ATS.Service.Messages;
using ATS.Web.Infrastructure.Helpers;
using ATS.Web.Models;
using Syncfusion.JavaScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ATS.Web.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
      
        private readonly IEmployeeService _employeeService;
        private readonly INotify _notify;

        public EmployeeController(IEmployeeService employeeService, INotify notify)
        {
            _employeeService = employeeService;
            _notify = notify;
        }
        // GET: Employee
        //[NECAuthorize(Key = new string[] { NECPermissions.EmployeeRead })]
        public ActionResult Index()
        {
            return View();
        }

       // [NECAuthorize(Key = new string[] { NECPermissions.EmployeeCreate })]
        public ActionResult AddEmployee()
        {
            EmployeeViewModel viewModel = new EmployeeViewModel();
            return View(viewModel);
        }
        [HttpPost]
      //  [NECAuthorize(Key = new string[] { NECPermissions.EmployeeCreate })]
        public ActionResult AddEmployee(EmployeeViewModel employeeviewmodel, HttpPostedFileBase profile)
        {
            if (!ModelState.IsValid)
                return View(employeeviewmodel);

            var employee = new ATS.Core.Domain.DTO.EmployeeViewModel();

            employee.EmployeeCode = employeeviewmodel.EmployeeCode;
            employee.Name = employeeviewmodel.Name;
            employee.Email = employeeviewmodel.Email;
            employee.DepartmentID = employeeviewmodel.DepartmentID;
            employee.DesignationID = employeeviewmodel.DesignationID;
            employee.Basic = employeeviewmodel.Basic;
            employee.SplAllowance = employeeviewmodel.SplAllowance;
            employee.Col = employeeviewmodel.Col;
            employee.OthersAllowance = employeeviewmodel.OthersAllowance;
            employee.Conveyance = employeeviewmodel.Conveyance;
            employee.DORJ = employeeviewmodel.DORJ;
            employee.Housing = employeeviewmodel.Housing;
            employee.Gross = employeeviewmodel.Gross;
            employee.OTThreshold = employeeviewmodel.OTThreshold;
            employee.IsOTEligible = employeeviewmodel.IsOTEligible == 1 ? true : false;
            employee.WeekOffMain = employeeviewmodel.WeekOffMain;
            employee.WeeklyOffAlternate = employeeviewmodel.WeeklyOffAlternate;
            employee.ShiftCode = employeeviewmodel.ShiftCode;
            employee.CreatedBy = 1;
            employee.CreatedOn = DateTime.Now;
            employee.IsActive = true;
            var response = _employeeService.AddEmployee(employee);
            if (response)
            {
                SuccessNotification(Convert.ToString("Employee Added Successfully"));
                return RedirectToAction("Index");
            }
            else
            {
                ErrorNotification("Unable to Add Employee.Please Tyr again");
            }


            //ErrorNotification(response.Message);

            return View(employeeviewmodel);
        }
        //[NECAuthorize(Key = new string[] { NECPermissions.EmployeeUpdate })]
        public ActionResult Detail(long id = 0)
        {
            ATS.Core.Domain.DTO.EmployeeViewModel employeeviewmodel = new ATS.Core.Domain.DTO.EmployeeViewModel();
            EmployeeViewModel employee = new EmployeeViewModel();
            if (id > 0)
            {
                employeeviewmodel = _employeeService.GetEmployeeByID(id);
                employee.EmployeeCode = employeeviewmodel.EmployeeCode;
                employee.Name = employeeviewmodel.Name;
                employee.Email = employeeviewmodel.Email;
                employee.DepartmentID = employeeviewmodel.DepartmentID;
                employee.DesignationID = employeeviewmodel.DesignationID;
                employee.Basic = employeeviewmodel.Basic;
                employee.SplAllowance = employeeviewmodel.SplAllowance;
                employee.Col = employeeviewmodel.Col;
                employee.OthersAllowance = employeeviewmodel.OthersAllowance;
                employee.Conveyance = employeeviewmodel.Conveyance;
                employee.DORJ = employeeviewmodel.DORJ;
                employee.Housing = employeeviewmodel.Housing;
                employee.Gross = employeeviewmodel.Gross;
                employee.OTThreshold = employeeviewmodel.OTThreshold;
                employee.IsOTEligible = employeeviewmodel.IsOTEligible == true ? 1 : 2;
                employee.WeekOffMain = employeeviewmodel.WeekOffMain;
                employee.ShiftCode = employeeviewmodel.ShiftCode;
                employee.WeeklyOffAlternate = employeeviewmodel.WeeklyOffAlternate;
                employee.CreatedBy = employeeviewmodel.CreatedBy;
                
                employee.IsActive = true;
            }
            return View(employee);
        }

        public ActionResult Delete(long id = 0)
        {
            var result = _employeeService.Delete(id);
            if (result)
            {            
                SuccessNotification("Successfully Deleted Employee");
            }
            else
                ErrorNotification("Unable to Delete Employee.Please Tyr again");
            return View("Index");
        }

        [HttpPost]
        //[NECAuthorize(Key = new string[] { NECPermissions.EmployeeUpdate })]
        public ActionResult Detail(EmployeeViewModel employeeviewmodel)
        {
            var employee = _employeeService.GetEmployee(employeeviewmodel.Id);
        
            if (employee != null)
            {
                employee.EmployeeCode = employeeviewmodel.EmployeeCode;
                employee.Name = employeeviewmodel.Name;
                employee.Email = employeeviewmodel.Email;
                employee.DepartmentID = employeeviewmodel.DepartmentID;
                employee.DesignationID = employeeviewmodel.DesignationID;
                employee.Basic = employeeviewmodel.Basic;
                employee.SplAllowance = employeeviewmodel.SplAllowance;
                employee.Col = employeeviewmodel.Col;
                employee.OthersAllowance = employeeviewmodel.OthersAllowance;
                employee.Conveyance = employeeviewmodel.Conveyance;
                employee.DORJ = employeeviewmodel.DORJ;
                employee.Housing = employeeviewmodel.Housing;
                employee.Gross = employeeviewmodel.Gross;
                employee.OTThreshold = employeeviewmodel.OTThreshold;
                employee.IsOTEligible = employeeviewmodel.IsOTEligible == 1 ? true : false;                    
                employee.WeekOffMain = employeeviewmodel.WeekOffMain;
                employee.ShiftCode = employeeviewmodel.ShiftCode;
                employee.WeeklyOffAlternate = employeeviewmodel.WeeklyOffAlternate;
                employee.CreatedBy = 1;
                employee.CreatedOn = DateTime.Now;
                employee.IsActive = true;
                _employeeService.UpdateEmployee(employee);
                SuccessNotification("Employee Successfully Updated");
            }
            else
            {
                ErrorNotification("Something Went Wrong!Please Try Again");
                return RedirectToAction("Index");
            }
            //if (!_notify.HasErrors)
            //{                
            //    return RedirectToAction("Index");
            //}

            return RedirectToAction("Index");
        }
    //    [NECAuthorize(Key = new string[] { NECPermissions.EmployeeRead })]
        public JsonResult EmployeeData(DataManager dm)
        {
            if (dm.Search != null && dm.Search.Count > 0)
            {

            }
            var employeedata = _employeeService.GetAllEmployee(dm.Skip, dm.Take);                                    
            return Json(new { result = employeedata.Data, count = employeedata.Count }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetEmployeeDetails(string empID)
        {
            EmployeeViewModel model = new EmployeeViewModel();
            if (!string.IsNullOrWhiteSpace(empID))
            {
                model.Id = Convert.ToInt64(empID);
            }
            var employeemodel =  _employeeService.GetEmployeeByID(model.Id);
            return Json(employeemodel, JsonRequestBehavior.AllowGet);
        }
        // Action added by Shoeb
        public ActionResult GetAllEmployee()
        {
            return View();
        }
      

      
    }
}