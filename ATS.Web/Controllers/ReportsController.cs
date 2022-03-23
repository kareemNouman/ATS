using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;
using ATS.Service.Reports;
using Syncfusion.JavaScript;

namespace ATS.Web.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly IReportsService _reportService;
        public ReportsController(IReportsService reportService)
        {
            this._reportService = reportService;
        }
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        #region SingleEmployeeAttendanceReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult SingleEmployeeReport()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            return View(model);
        }

        public ActionResult SingleEmployeeReportDataSource(DataManager dm, string startdate, string enddate, string departmentid,string employeecode,string employeename, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            departmentid = string.IsNullOrWhiteSpace(departmentid) ? null : departmentid;
            employeecode = string.IsNullOrWhiteSpace(employeecode) ? null : employeecode;
            employeename = string.IsNullOrWhiteSpace(employeename) ? null : employeename;

            if (pdf == "true")
            {
                request.PageSize = 1700;
                request.Page = 0;
            }
            else
            {
                request.PageSize = string.IsNullOrWhiteSpace(print) ? dm.Take : 1700;
                request.Page = string.IsNullOrWhiteSpace(print) ? dm.Skip : 0;
            }
            request.Filters.Add("startdate", startdate);
            request.Filters.Add("enddate", enddate);
            request.Filters.Add("departmentid", departmentid);
            request.Filters.Add("employeecode", employeecode);
            request.Filters.Add("employeename", employeename);


            var response =  _reportService.SingleEmployeeAttendanceReport(request);

            //var responseData = response;
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;                
                return new Rotativa.ViewAsPdf("_singleEmployeeReportsPartial", response.Results);
            }
            return PartialView("_singleEmployeeReportsPartial", response.Results);
        }


        public ActionResult SingleEmployeeReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_singleEmployeeReportsPartial", response.Results);
        }

        #endregion

        #region AllEmployeeAttendanceReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult AllEmployeesReport()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            return View(model);
        }

        public ActionResult AllEmployeesReportDataSource(DataManager dm, string startdate, string enddate, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            //departmentid = string.IsNullOrWhiteSpace(departmentid) ? null : departmentid;
            //employeecode = string.IsNullOrWhiteSpace(employeecode) ? null : employeecode;
            //employeename = string.IsNullOrWhiteSpace(employeename) ? null : employeename;

            if (pdf == "true")
            {
                request.PageSize = 1700;
                request.Page = 0;
            }
            else
            {
                request.PageSize = string.IsNullOrWhiteSpace(print) ? dm.Take : 1700;
                request.Page = string.IsNullOrWhiteSpace(print) ? dm.Skip : 0;
            }
            request.Filters.Add("startdate", startdate);
            request.Filters.Add("enddate", enddate);
            //request.Filters.Add("departmentid", departmentid);
            //request.Filters.Add("employeecode", employeecode);
            //request.Filters.Add("employeename", employeename);


            var response = _reportService.AllEmployeeAttendanceReport(request);

            //var responseData = response;
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_allEmployeeReportsPartial", response.Results);
            }
            return PartialView("_allEmployeeReportsPartial", response.Results);
        }


        public ActionResult AllEmployeesReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_singleEmployeeReportsPartial", response.Results);
        }

        #endregion

        #region OverTimeEmployeeAttendanceReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult OverTimeEmployeeReport()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            return View(model);
        }

        public ActionResult OverTimeEmployeeReportDataSource(DataManager dm, string startdate, string enddate, string departmentid, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            departmentid = string.IsNullOrWhiteSpace(departmentid) ? null : departmentid;            

            if (pdf == "true")
            {
                request.PageSize = 1700;
                request.Page = 0;
            }
            else
            {
                request.PageSize = string.IsNullOrWhiteSpace(print) ? dm.Take : 1700;
                request.Page = string.IsNullOrWhiteSpace(print) ? dm.Skip : 0;
            }
            request.Filters.Add("startdate", startdate);
            request.Filters.Add("enddate", enddate);
            request.Filters.Add("departmentid", departmentid);           


            var response = _reportService.OverTimeEmployeeAttendanceReport(request);
            
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_singleEmployeeReportsPartial", response.Results);
            }
            return PartialView("_singleEmployeeReportsPartial", response.Results);
        }


        public ActionResult OverTimeEmployeeReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_singleEmployeeReportsPartial", response.Results);
        }

        #endregion

        #region PayrollEmployeeReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult EmployeePayrollReport()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            return View(model);
        }

        public ActionResult EmployeePayrollReportDataSource(DataManager dm, string startdate, string enddate, string departmentid, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            departmentid = string.IsNullOrWhiteSpace(departmentid) ? null : departmentid;

            if (pdf == "true")
            {
                request.PageSize = 1700;
                request.Page = 0;
            }
            else
            {
                request.PageSize = string.IsNullOrWhiteSpace(print) ? dm.Take : 1700;
                request.Page = string.IsNullOrWhiteSpace(print) ? dm.Skip : 0;
            }
            request.Filters.Add("startdate", startdate);
            request.Filters.Add("enddate", enddate);
            request.Filters.Add("departmentid", departmentid);


            var response = _reportService.OverTimeEmployeeAttendanceReport(request);

            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_singleEmployeeReportsPartial", response.Results);
            }
            return PartialView("_singleEmployeeReportsPartial", response.Results);
        }


        public ActionResult EmployeePayrollReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_singleEmployeeReportsPartial", response.Results);
        }
        #endregion
    }
}