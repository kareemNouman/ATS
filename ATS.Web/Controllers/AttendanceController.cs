using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATS.Service.DailyAttendance;
using ATS.Service.Masters;
using ATS.Service.Messages;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using OfficeOpenXml;
using Syncfusion.JavaScript;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;

namespace ATS.Web.Controllers
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        // GET: Attendance
        private readonly IDailyAttendanceService _dailyAttendanceService;
        private readonly INotify _notify;
        public AttendanceController(IDailyAttendanceService dailyAttendanceService, INotify notify)
        {
            _dailyAttendanceService = dailyAttendanceService;
            _notify = notify;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(long id = 0)
        {
            ATS.Core.Domain.DTO.AttendanceViewModel viewmodel = new ATS.Core.Domain.DTO.AttendanceViewModel();
            if (id > 0)
            {
                viewmodel = _dailyAttendanceService.GetAttendanceByID(id);
            }
            return View(viewmodel);
        }

        [HttpPost]
        //[NECAuthorize(Key = new string[] { NECPermissions.EmployeeUpdate })]
        public ActionResult Detail(AttendanceViewModel viewmodel)
        {
            var attendance = _dailyAttendanceService.GetAttendance(viewmodel.Id);

            if (attendance != null)
            {
                attendance.EmployeeCode = viewmodel.EmployeeCode;
                attendance.Name = viewmodel.Name;
                attendance.TimeIn = !string.IsNullOrWhiteSpace(viewmodel.TimeIn) ? viewmodel.TimeIn.Replace('.',':'): null;
                attendance.TimeOut = !string.IsNullOrWhiteSpace(viewmodel.TimeOut) ? viewmodel.TimeOut.Replace('.', ':') : null;
                attendance.TotalHours = viewmodel.TotalHours;
                attendance.OT1 = viewmodel.OT1;
                attendance.OT2 = viewmodel.OT2;
                attendance.OT3 = viewmodel.OT3;
                attendance.OT4 = viewmodel.OT4;
                attendance.CreatedOn = DateTime.Now;
                attendance.IsActive = true;
                _dailyAttendanceService.UpdateAttendance(attendance);
                SuccessNotification("Attendance Successfully Updated");
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


        public ActionResult AttendanceDataSource(DataManager dm, string employeename)
        {
            GridRequestModel request = new GridRequestModel();

            //startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            //enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            //departmentid = string.IsNullOrWhiteSpace(departmentid) ? null : departmentid;
            //employeecode = string.IsNullOrWhiteSpace(employeecode) ? null : employeecode;
            employeename = string.IsNullOrWhiteSpace(employeename) ? null : employeename;
            request.PageSize = dm.Take;
            request.Page = dm.Skip;
            //if (pdf == "true")
            //{
            //    request.PageSize = 1700;
            //    request.Page = 0;
            //}
            //else
            //{
            //    request.PageSize = string.IsNullOrWhiteSpace(print) ? dm.Take : 1700;
            //    request.Page = string.IsNullOrWhiteSpace(print) ? dm.Skip : 0;
            //}
            //request.Filters.Add("startdate", startdate);
            //request.Filters.Add("enddate", enddate);
            //request.Filters.Add("departmentid", departmentid);
            //request.Filters.Add("employeecode", employeecode);
            request.Filters.Add("employeename", employeename);


            var response = _dailyAttendanceService.AttendanceDataRequest(request);
            return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            //var responseData = response;
            //if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                
            //else if (!string.IsNullOrWhiteSpace(pdf))
            //{
            //    ViewBag.IsPDF = true;
            //    return new Rotativa.ViewAsPdf("_singleEmployeeReportsPartial", response.Results);
            //}
            //return PartialView("_singleEmployeeReportsPartial", response.Results);
        }


        //public ActionResult AttendanceDataSourcePDF()
        //{
        //    GridRequestModel request = new GridRequestModel();

        //    string print = "";

        //    request.PageSize = 1000;
        //    request.Page = 0;
        //    var response = _reportService.SingleEmployeeAttendanceReport(request);

        //    return new Rotativa.PartialViewAsPdf("_singleEmployeeReportsPartial", response.Results);
        //}

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase excelfile)
        {

            if (excelfile == null)
            {
                ErrorNotification("Cannot find a file to generate bills", true);
                return RedirectToAction("Index");
            }

            //if (excelfile.FileName.EndsWith("xlsx"))
            //{
            //    ErrorNotification("unsupported excel file format", true);
            //    return RedirectToAction("BulkUpload");
            //}

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
                            TotalHours = Convert.ToDouble(workSheet.Cells[i, 7].Value),
                            OT1 = Convert.ToDecimal(workSheet.Cells[i, 8].Value),
                            OT2 = Convert.ToDecimal(workSheet.Cells[i, 9].Value),
                            OT3 = Convert.ToDecimal(workSheet.Cells[i, 10].Value),
                            OT4 = Convert.ToDecimal(workSheet.Cells[i, 11].Value),
                            Status = Convert.ToString(workSheet.Cells[i, 12].Value),
                            Date = DateTime.Now,
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
    }
}