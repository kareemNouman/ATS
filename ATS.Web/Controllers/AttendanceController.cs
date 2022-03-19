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
                            TotalHours = Convert.ToDecimal(workSheet.Cells[i, 7].Value),
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