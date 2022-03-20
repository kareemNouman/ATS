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

                for (int i = 7; i <= lastRow; i++)
                {
                    var SLNo = Convert.ToInt64(workSheet.Cells[i, 1].Value);
                    if (SLNo != 0)
                    {
                        var attendance = new ATS.Core.Domain.DTO.AttendanceViewModel
                        {
                            EmployeeCode = Convert.ToInt64(workSheet.Cells[i, 2].Value),
                            Name = Convert.ToString(workSheet.Cells[i, 3].Value),
                            Designation = Convert.ToString(workSheet.Cells[i, 4].Value),
                            TimeIn = Convert.ToString(workSheet.Cells[i, 5].Value),
                            TimeOut = Convert.ToString(workSheet.Cells[i, 6].Value),
                            TotalHours = Convert.ToDecimal(workSheet.Cells[i, 7].Value),
                            OT1 = Convert.ToDecimal(workSheet.Cells[i, 8].Value),
                            OT2 = Convert.ToDecimal(workSheet.Cells[i, 9].Value),
                            OT3 = Convert.ToDecimal(workSheet.Cells[i, 10].Value),
                            OT4 = Convert.ToDecimal(workSheet.Cells[i, 11].Value),
                            Status = Convert.ToString(workSheet.Cells[i, 12].Value),
                            Date = Convert.ToDateTime(attendancedate),
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            IsActive = true
                        };

                        _dailyAttendanceService.AddAttendance(attendance);
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
        

        public ActionResult Roles()
        {
            var roles = new List<SelectListItem>
                                    {
                            new SelectListItem{ Text="Admin", Value = "1" },
                            new SelectListItem{ Text="Attendance Tracker", Value = "2" },
                                    };
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

    }
}