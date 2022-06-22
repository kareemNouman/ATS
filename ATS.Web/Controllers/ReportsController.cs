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

        public ActionResult SingleEmployeeReportDataSource(DataManager dm, string startdate, string enddate, string departmentname,string employeecode,string employeename, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            departmentname = string.IsNullOrWhiteSpace(departmentname) ? null : departmentname;
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
            request.Filters.Add("departmentname", departmentname);
            request.Filters.Add("employeecode", employeecode);
            request.Filters.Add("employeename", employeename);

            PagedResults<AttendanceViewModel> response = new PagedResults<AttendanceViewModel>();
            if (startdate != null || enddate != null || departmentname != null ||
                employeecode != null || employeename != null)
            {
                response = _reportService.SingleEmployeeAttendanceReport(request);
            }
            else
            {
                request.Filters.Add("noemployeename", "NoEmployee");
                response = _reportService.SingleEmployeeAttendanceReport(request);
            }
            

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
            EmployeeViewModel model = new EmployeeViewModel();
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
                return Json(new { result = response, count = response.Select(x => x.TotalRecords).FirstOrDefault() });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;

                //string header = Url.Action("Header","Reports", Request.Url.Scheme);
                    //Url.Action("Header","Reports",new { date = response.Select(x => x.MonthDate).FirstOrDefault() }); //Server.MapPath("~/bin/PrintHeader.html");//Path of PrintHeader.html File
                //string footer = Server.MapPath("~/Views/Shared/Footer.html");//Server.MapPath("~/bin/PrintFooter.html");//Path of PrintFooter.html File

                //string customSwitches = string.Format("--header-html  \"{0}\" " +
                //                       "--header-spacing \"0\" " +                                       
                //                       "--header-font-size \"10\" ", header);

                return new Rotativa.ViewAsPdf("_allEmployeeReportsPartial", response) 
                {
                    PageOrientation =Rotativa.Options.Orientation.Landscape,
                    //CustomHeaders = Rotativa.Options.
                    PageSize = Rotativa.Options.Size.A4,
                    //CustomSwitches = customSwitches
                };
            }
            return PartialView("_allEmployeeReportsPartial", response);
        }


        public ActionResult AllEmployeesReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_allEmployeeReportsPartial", response.Results);
        }

        #endregion

        #region OverTimeEmployeeAttendanceReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult OverTimeEmployeeReport()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            return View(model);
        }

        public ActionResult OverTimeEmployeeReportDataSource(DataManager dm, string startdate, string enddate, string departmentname, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            departmentname = string.IsNullOrWhiteSpace(departmentname) ? null : departmentname;            

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
            request.Filters.Add("departmentname", departmentname);           


            var response = _reportService.OverTimeEmployeeAttendanceReport(request);
            
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_dailyOTApproveReportsPartial", response.Results);
            }
            return PartialView("_dailyOTApproveReportsPartial", response.Results);
        }


        public ActionResult OverTimeEmployeeReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_dailyOTApproveReportsPartial", response.Results);
        }

        #endregion

        #region PayrollEmployeeReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult EmployeePayrollReport()
        {
            EmployeeViewModel model = new EmployeeViewModel();
            return View(model);
        }

        public ActionResult EmployeePayrollReportDataSource(DataManager dm, string startdate, string enddate,string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            

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
         

            var response = _reportService.EmployeePayrollReport(request);

            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response, count = response.Select(x=>x.TotalRecords).FirstOrDefault() });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_employeePayrollReportsPartial", response);
            }
            return PartialView("_employeePayrollReportsPartial", response);
        }


        public ActionResult EmployeePayrollReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_employeePayrollReportsPartial", response.Results);
        }
        #endregion

        #region EmployeeLeaveReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult EmployeeLeaveReport()
        {
            EmployeeLeaveViewModel model = new EmployeeLeaveViewModel();
            return View(model);
        }

        public ActionResult EmployeeLeaveReportDataSource(DataManager dm, string startdate, string enddate, string departmentid, string employeecode, string employeename, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;            
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

            var response = _reportService.EmployeeLeaveReport(request);

            //var responseData = response;
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_employeeLeaveReportsPartial", response.Results);
            }
            return PartialView("_employeeLeaveReportsPartial", response.Results);
        }


        public ActionResult EmployeeLeaveReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.EmployeeLeaveReport(request);

            return new Rotativa.PartialViewAsPdf("_employeeLeaveReportsPartial", response.Results);
        }

        #endregion

        #region DailyEmployeeAttendanceReport
        //[NECAuthorize(Key = new string[] { NECPermissions.SalaryReport })]

        public ActionResult DailyEmployeeReport()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            return View(model);
        }

        public ActionResult DailyEmployeeReportDataSource(DataManager dm, string startdate, string enddate, string employeename, string departmentname, string print, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;
            departmentname = string.IsNullOrWhiteSpace(departmentname) ? null : departmentname;            
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
            request.Filters.Add("departmentname", departmentname);            
            request.Filters.Add("employeename", employeename);


            var response = _reportService.DailyEmployeeAttendanceReport(request);

            //var responseData = response;
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response.Results, count = response.TotalNumberOfRecords });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_dailyAttendanceAllEmployeeReportsPartial", response.Results);
            }
            return PartialView("_dailyAttendanceAllEmployeeReportsPartial", response.Results);
        }

        public ActionResult DailyEmployeeReportPDF()
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

        public ActionResult AllEmployeesMinistryReport()
        {
            EmployeeViewModel model = new EmployeeViewModel();
            return View(model);
        }

        public ActionResult AllEmployeesMinistryReportDataSource(DataManager dm, string startdate, string enddate, string print, string pdf)
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
                return Json(new { result = response, count = response.Select(x => x.TotalRecords).FirstOrDefault() });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_allEmployeeReportsPartial", response);
            }
            return PartialView("_allEmployeeReportsPartial", response);
        }


        public ActionResult AllEmployeesMinistryReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.SingleEmployeeAttendanceReport(request);

            return new Rotativa.PartialViewAsPdf("_singleEmployeeReportsPartial", response.Results);
        }

        #endregion

        #region CashOvertimeReport
        public ActionResult CashIncentiveEmployeesReport()
        {
            EmployeeViewModel model = new EmployeeViewModel();
            return View(model);
        }

        public ActionResult CashIncentiveEmployeesReportDataSource(DataManager dm, string startdate, string enddate, string print, string pdf)
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


            var response = _reportService.CashIncentivesReport(request);

            //var responseData = response;
            if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
                return Json(new { result = response, count = response.Select(x => x.TotalRecords).FirstOrDefault() });
            else if (!string.IsNullOrWhiteSpace(pdf))
            {
                ViewBag.IsPDF = true;
                return new Rotativa.ViewAsPdf("_cashIncentiveEmployeeReportsPartial", response);
            }
            return PartialView("_cashIncentiveEmployeeReportsPartial", response);
        }


        public ActionResult CashIncentiveEmployeesReportPDF()
        {
            GridRequestModel request = new GridRequestModel();

            string print = "";

            request.PageSize = 1000;
            request.Page = 0;
            var response = _reportService.CashIncentivesReport(request);

            return new Rotativa.PartialViewAsPdf("_cashIncentiveEmployeeReportsPartial", response);
        }


        #endregion

        #region PayslipGeneration


        public ActionResult GenerateMonthlySlips()
        {
            return View();
        }

        public ActionResult GeneratePaySlip(string startdate, string enddate, string pdf)
        {
            GridRequestModel request = new GridRequestModel();

            startdate = string.IsNullOrWhiteSpace(startdate) ? null : startdate;
            enddate = string.IsNullOrWhiteSpace(enddate) ? null : enddate;


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
            request.Filters.Add("startdate", startdate);
            request.Filters.Add("enddate", enddate);


            var response = _reportService.EmployeePayrollReport(request);

            //if (string.IsNullOrWhiteSpace(print) && string.IsNullOrWhiteSpace(pdf))
            //    return Json(new { result = response, count = response.Select(x => x.TotalRecords).FirstOrDefault() });
            //else if (!string.IsNullOrWhiteSpace(pdf))
            //{
            //    ViewBag.IsPDF = true;
            //    return new Rotativa.ViewAsPdf("_singleEmployeeReportsPartial", response);
            //}
            return new Rotativa.ViewAsPdf("_generatePayslipsReportsPartial", response);
            //return PartialView("_singleEmployeeReportsPartial", response);
        }

        public ActionResult Header()
        {
            //ViewBag.Date = date;
            return PartialView("_Header");
        }

        #endregion
    }
}