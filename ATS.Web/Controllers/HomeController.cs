using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATS.Service.DailyAttendance;
using ATS.Service.Employees;
using ATS.Service.Masters;
using ATS.Service.Messages;
using Newtonsoft.Json;
using OfficeOpenXml;
using Syncfusion.JavaScript;


namespace ATS.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IMasterService _masterService;
        private readonly IEmployeeService _employeeService;
        private readonly IDailyAttendanceService _dailyAttendanceService;
        private readonly INotify _notify;
        public HomeController(IMasterService masterService, IDailyAttendanceService dailyAttendanceService, IEmployeeService employeeService, INotify notify)
        {
            _masterService = masterService;
            _employeeService = employeeService;
            _dailyAttendanceService = dailyAttendanceService;
            _notify = notify;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase excelfile, FormCollection form)
        {
            string attendancedate = form["attendancedate"].Replace(",", "");
            if (excelfile == null)
            {
                ErrorNotification("Cannot find a file to generate attendance", true);
                return RedirectToAction("Index");
            }

            var location = Server.MapPath("~/Uploads/exceluploads/" + DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetExtension(excelfile.FileName));
            excelfile.SaveAs(location);

            List<ATS.Core.Domain.DTO.AttendanceViewModel> readings = new List<ATS.Core.Domain.DTO.AttendanceViewModel>();
            var file = new FileInfo(location);
            if (file.Extension == ".xls")
            {
                file = ConvertXLS_XLSX(file.FullName);
            }


            using (var excelpackage = new ExcelPackage(file))
            {
                var workSheet = excelpackage.Workbook.Worksheets[0];
                if (workSheet == null)
                {
                    ErrorNotification("File was empty", true);
                    return RedirectToAction("Index");
                }

                var lastRow = workSheet.Dimension.End.Row;
                DeleteExistAttendance(Convert.ToDateTime(attendancedate));
                for (int i = 7; i <= lastRow; i++)
                {
                    var SLNo = Convert.ToInt64(workSheet.Cells[i, 1].Value);
                    if (SLNo != 0)
                    {
                        long empCode = Convert.ToInt64(workSheet.Cells[i, 2].Value);
                        var employee = empCode != 0 ? _employeeService.GetEmployeeByCode(empCode) : null;
                        var attendance = new ATS.Core.Domain.DTO.AttendanceViewModel
                        {
                            EmployeeCode = Convert.ToInt64(workSheet.Cells[i, 2].Value),
                            Name = Convert.ToString(workSheet.Cells[i, 3].Value),
                            Designation = employee.Designation,
                            Department = employee.Department,
                            //Designation = Convert.ToString(workSheet.Cells[i, 4].Value),
                            TimeIn = Convert.ToString(workSheet.Cells[i, 5].Value),
                            TimeOut = Convert.ToString(workSheet.Cells[i, 6].Value),
                            TotalHours = Convert.ToDouble(workSheet.Cells[i, 7].Value),
                            OT1 = employee.IsOTEligible == false ? Convert.ToDecimal(workSheet.Cells[i, 8].Value):0,
                            OT2 = employee.IsOTEligible == false ? Convert.ToDecimal(workSheet.Cells[i, 9].Value):0,
                            OT3 = employee.IsOTEligible == false ? Convert.ToDecimal(workSheet.Cells[i, 10].Value):0,
                            OT4 = employee.IsOTEligible == false ? Convert.ToDecimal(workSheet.Cells[i, 11].Value): 0,
                            Status =  Convert.ToString(workSheet.Cells[i, 12].Value),
                            Date = Convert.ToDateTime(attendancedate),
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            IsActive = true
                        };
                        if (attendance.OT1 != 0 || attendance.OT2 != 0 || attendance.OT3 != 0 || attendance.OT4 != 0)
                        {

                            if (attendance.OT1 < 1)
                                attendance.OT1 = 0;
                            if (attendance.OT2 < 1)
                                attendance.OT2 = 0;
                            if (attendance.OT3 < 1)
                                attendance.OT3 = 0;
                            if (attendance.OT4 < 1)
                                attendance.OT4 = 0;
                        }
                        //If Intime or OutTime is empty to be executed.
                        if (string.IsNullOrWhiteSpace(attendance.TimeIn) || string.IsNullOrWhiteSpace(attendance.TimeOut))
                        {
                            if (!string.IsNullOrWhiteSpace(attendance.TimeIn) && string.IsNullOrWhiteSpace(attendance.TimeOut))
                            {
                                if (employee.ShiftCode == 1)
                                    attendance.TimeOut = "14:00";
                                if (employee.ShiftCode == 2)
                                    attendance.TimeOut = "22:00";
                                if (employee.ShiftCode == 3)
                                    attendance.TimeOut = "06:00";
                                if (employee.ShiftCode == 4)
                                    attendance.TimeOut = "17:00";
                            }
                            if (string.IsNullOrWhiteSpace(attendance.TimeIn) && !string.IsNullOrWhiteSpace(attendance.TimeOut))
                            {
                                if (employee.ShiftCode == 1)
                                    attendance.TimeIn = "06:00";
                                if (employee.ShiftCode == 2)
                                    attendance.TimeIn = "14:00";
                                if (employee.ShiftCode == 3)
                                    attendance.TimeIn = "22:00";
                                if (employee.ShiftCode == 4)
                                    attendance.TimeIn = "08:00";
                            }
                            double InTime = Convert.ToDouble(attendance.TimeOut.Replace(':', '.'));
                            double OutTime = Convert.ToDouble(attendance.TimeOut.Replace(':', '.'));
                            if (InTime == 22.00)
                            {
                                InTime = 22.00 - 8.00;
                            }
                            attendance.TotalHours = OutTime - InTime;
                        }                       
                        //_dailyAttendanceService.AddAttendance(attendance);
                    }

                }
            }


            if (_notify.HasErrors)
            {
                ErrorNotification("Unable to upload file.Please contact administrator", true);
                return RedirectToAction("Index");
                //return Json(new NECApiResponse { IsSuccess = false });
            }
            else
            {
                SuccessNotification("Attendance Save Successfully", true);
                return RedirectToAction("Index");
            }

        }

        public static FileInfo ConvertXLS_XLSX(string fileFullName)
        {
            var app = new Microsoft.Office.Interop.Excel.Application();
            var xlsFile = fileFullName;
            var wb = app.Workbooks.Open(xlsFile);
            var xlsxFile = xlsFile + "x";
            wb.SaveAs(Filename: xlsxFile, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            var file = new FileInfo(xlsxFile);
            wb.Close();
            app.Quit();
            return file;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

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
            return Json(departmentViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllDepartments(DataManager dm)
        {

            var searchText = "";

            if (dm.Where == null ? false : dm.Where.Count > 0)
                searchText = Convert.ToString(dm.Where[0].value);
            var departments = _masterService.GetAllDepartmentsForAutoSearch();          

            if (string.IsNullOrWhiteSpace(searchText))
                return Json(departments, JsonRequestBehavior.AllowGet);

            var list = departments.Where(x =>
            {
                if (string.IsNullOrWhiteSpace(x.Name))
                    return false;

                return x.Name.ToLower().Contains(searchText.ToLower());
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Employees(DataManager dm)
        {

            var searchText = "";

            if (dm.Where == null ? false : dm.Where.Count > 0)
                searchText = Convert.ToString(dm.Where[0].value);
            var employees = _employeeService.GetAllEmployee();

            if (string.IsNullOrWhiteSpace(searchText))
                return Json(employees, JsonRequestBehavior.AllowGet);

            var list = employees.Where(x =>
            {
                if (string.IsNullOrWhiteSpace(x.Name))
                    return false;

                return x.Name.ToLower().Contains(searchText.ToLower());
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

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
            return Json(designationViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WeekOffMain()
        {
            var weekOffMain = new List<SelectListItem>
                                    {
                              //new SelectListItem{ Text="Select", Value = "0" },
                            new SelectListItem{ Text="Sunday", Value = "1" },
                            new SelectListItem{ Text="Monday", Value = "2" },
                            new SelectListItem{ Text="Tuesday", Value = "3" },
                            new SelectListItem{ Text="Wednesday", Value = "4" },
                            new SelectListItem{ Text="Thursday", Value = "5" },
                            new SelectListItem{ Text="Friday", Value = "6" },
                             new SelectListItem{ Text="Saturday", Value = "7" },
                                    };
            return Json(weekOffMain, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OTEligible()
        {
            var isOTEligible = new List<SelectListItem>
                                    {                              
                            new SelectListItem{ Text="Applicable", Value = "1" },
                            new SelectListItem{ Text="Not Applicable", Value = "2" },                            
                                    };
            return Json(isOTEligible, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AttendanceData(DataManager dm)
        {
            if (dm.Search  != null &&  dm.Search.Count > 0)
            {

            }
            var attendancedata = _dailyAttendanceService.GetAllAttendance(dm.Skip, dm.Take);
            return Json(new { result = attendancedata.Data, count = attendancedata.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeShifts()
        {
            var weekOffMain = new List<SelectListItem>
                                    {
                              //new SelectListItem{ Text="Select", Value = "0" },
                            new SelectListItem{ Text="A shift (6am to 2pm)", Value = "1" },
                            new SelectListItem{ Text="B shift (2pm to 10pm)", Value = "2" },
                            new SelectListItem{ Text="C shift (10pm to 6am)", Value = "3" },
                            new SelectListItem{ Text="G shift (8am to 5pm)", Value = "4" },                            
                                    };
            return Json(weekOffMain, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Roles()
        {
            var roles = new List<SelectListItem>
                                    {
                            new SelectListItem{ Text="Admin", Value = "1" },
                            new SelectListItem{ Text="Attendance Tracker", Value = "2" },
                                    };
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LeaveTypes()
        {
            var leaves = _masterService.GetAllLeaves();
            List<ATS.Core.Domain.DTO.LeavesViewModel> viewModel = new List<ATS.Core.Domain.DTO.LeavesViewModel>();
            foreach (var item in leaves)
            {
                ATS.Core.Domain.DTO.LeavesViewModel model = new ATS.Core.Domain.DTO.LeavesViewModel();
                model.ID = item.ID;
                model.Name = item.Name;
                model.IsDelete = item.IsDelete;
                viewModel.Add(model);
            }
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAttendanceByDate(DateTime attendanceDate)
        {
            var attendance = _dailyAttendanceService.GetAttendanceByDate(attendanceDate);
            if (attendance == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public bool DeleteExistAttendance(DateTime date)
        {
            var result = false;

            var attendance = _dailyAttendanceService.GetAttendanceByDate(date);

            if (attendance != null)
            {
                result = _dailyAttendanceService.DeleteByDate(date);
            }
            return result;
        }

    }
}