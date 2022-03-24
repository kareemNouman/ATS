using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;
using System.Data.Entity;
using ATS.Data;
using ATS.Service.DailyAttendance;

namespace ATS.Service.Reports
{
    public class ReportsService: BaseService,IReportsService
    {
        private readonly IGenericRepository<Attendance> _attendanceRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IDailyAttendanceService _dailyAttendanceService;

        public ReportsService(IGenericRepository<Attendance> attendanceRepository, IGenericRepository<Employee> employeeRepository, IDailyAttendanceService dailyAttendanceService)
        {
            this._attendanceRepository = attendanceRepository;
            this._dailyAttendanceService = dailyAttendanceService;
            this._employeeRepository = employeeRepository;
        }

        public PagedResults<AttendanceViewModel> SingleEmployeeAttendanceReport(GridRequestModel request)
        {

            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            var departmentid = "";
            var employeecode = "";
            var employeename = "";

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            request.Filters.TryGetValue("departmentid", out departmentid);
            request.Filters.TryGetValue("employeecode", out employeecode);
            request.Filters.TryGetValue("employeename", out employeename);

            DateTime startDate;
            DateTime endDate;
            long departmentID;            
            long employeeCode;

            if (DateTime.TryParse(startdate, out startDate) && DateTime.TryParse(enddate, out endDate))
            {
                if (startdate == enddate)
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(endDate));
                }
                else
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(endDate));
            }

            //if (long.TryParse(departmentid, out departmentID))
            //    query = query.Where(x => x.Designation == departmentid);

            if (long.TryParse(employeecode, out employeeCode))
                query = query.Where(x => x.EmployeeCode == employeeCode);

            if (!string.IsNullOrWhiteSpace(employeename))
            {
                query = query.Where(x => x.Name.ToLower() == employeename.ToLower());
            }

            var total = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            {
                Date = x.Date,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                Status = x.Status,
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3= x.OT3,
                OT4 =x.OT4,
                TotalOT = x.OT1 + x.OT2 + x.OT3 + x.OT4,
                Total = total
            });

        }

        public PagedResults<AttendanceViewModel> AllEmployeeAttendanceReport(GridRequestModel request)
        {

            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            //var departmentid = "";
            //var employeecode = "";
            //var employeename = "";

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            //request.Filters.TryGetValue("departmentid", out departmentid);
            //request.Filters.TryGetValue("employeecode", out employeecode);
            //request.Filters.TryGetValue("employeename", out employeename);

            DateTime startDate;
            DateTime endDate;
            //long departmentID;
            //long employeeCode;

            if (DateTime.TryParse(startdate, out startDate) && DateTime.TryParse(enddate, out endDate))
            {
                if (startdate == enddate)
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(endDate));
                }
                else
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(endDate));
            }

            //if (long.TryParse(departmentid, out departmentID))
            //    query = query.Where(x => x.Designation == departmentid);

            //if (long.TryParse(employeecode, out employeeCode))
            //    query = query.Where(x => x.EmployeeCode == employeeCode);

            //if (!string.IsNullOrWhiteSpace(employeename))
            //{
            //    query = query.Where(x => x.Name.ToLower() == employeename.ToLower());
            //}

            //var total = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);

            //query = query.GroupBy(x => x.Name);        
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            {
               // PayToAttendance = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value),
                EmployeeCode = x.EmployeeCode,
                Name = x.Name,
                Designation = x.Designation,
                SickLeaves = 1,
                AdjustAnnLeaves =0,
                Absent = 0,
                ToPay = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalPayToDays,
                OT1 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT1,
                OT2 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT2,
                OT3 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT3,
                OT4 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT4
                //TotalOT = 
            });

        }

        public PagedResults<AttendanceViewModel> OverTimeEmployeeAttendanceReport(GridRequestModel request)
        {

            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            var departmentid = "";          

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            request.Filters.TryGetValue("departmentid", out departmentid);          

            DateTime startDate;
            DateTime endDate;
            long departmentID;

            if (DateTime.TryParse(startdate, out startDate) && DateTime.TryParse(enddate, out endDate))
            {
                if (startdate == enddate)
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(endDate));
                }
                else
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(endDate));
            }

            if (long.TryParse(departmentid, out departmentID))
                query = query.Where(x => x.Designation == departmentid);

            

           // var total = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            {
                EmployeeCode= x.EmployeeCode,
                Name = x.Name,
                Date = x.Date,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                Status = x.Status,
                OT1 = x.OT1 != null ? x.OT1.Value : 0,
                OT2 = x.OT2 != null ? x.OT2.Value : 0,
                OT3 = x.OT3 != null ? x.OT3.Value : 0,
                OT4 = x.OT4 != null ? x.OT4.Value : 0,
                TotalOT =  (x.OT1 != null ? x.OT1.Value : 0  + x.OT2 != null ? x.OT2.Value : 0
                            + x.OT3  != null ? x.OT3.Value : 0 + x.OT4 != null ? x.OT4.Value : 0),      
            });

        }

        public List<EmployeeViewModel> EmployeePayrollReport(GridRequestModel request)
        {                         
            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            //var departmentid = "";

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            //request.Filters.TryGetValue("departmentid", out departmentid);

            DateTime startDate;
            DateTime endDate;
            //long departmentID;

            if (DateTime.TryParse(startdate, out startDate) && DateTime.TryParse(enddate, out endDate))
            {
                if (startdate == enddate)
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(endDate));
                }
                else
                    query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(endDate));
            }
            var result = query.ToList();
            var employees = _employeeRepository.GetQueryable().OrderBy(x=>x.EmployeeCode).Skip(request.Page.Value).Take(request.PageSize.Value).Where(x => x.IsActive == true).ToList();            
            //if (long.TryParse(departmentid, out departmentID))
            //    query = query.Where(x => x.Designation == departmentid);
            var totalNumberOfRecords = _employeeRepository.GetQueryable().Count();
           // var pageSize = result.Count();
            List<EmployeeViewModel> viewModel = new List<EmployeeViewModel>();
            foreach (var item in employees)
            {
                EmployeeViewModel employee = new EmployeeViewModel();
                if (result.Where(x=>x.EmployeeCode== item.EmployeeCode).Count() > 0 )
                {
                    employee.Id = item.ID;
                    employee.EmployeeCode = item.EmployeeCode;
                    employee.Name = item.Name;
                    employee.Email = item.Email;
                    employee.DepartmentID = item.DepartmentID;
                    employee.DesignationID = item.DesignationID;
                    employee.Basic = item.Basic;
                    employee.SplAllowance = item.SplAllowance;
                    employee.Col = item.Col;
                    employee.OthersAllowance = item.OthersAllowance;
                    //employee.OthersAmount = item
                    //employee.BonusAmount = item
                    employee.Conveyance = item.Conveyance;
                    employee.DORJ = item.DORJ;
                    employee.Housing = item.Housing;
                    employee.Gross = item.Gross;
                    employee.OTThreshold = item.OTThreshold;
                    employee.IsOTEligible = item.IsOTEligible;
                    employee.OT1 = result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT1);
                    employee.OT2 = result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT2);
                    employee.OT3 = result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT3);
                    employee.OT4 = result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT4);
                    var totalOT1 = (employee.Gross != null ? employee.Gross.Value : 0) * (employee.OthersAllowance != null ? employee.OthersAllowance.Value : 0) * employee.OT1;
                    employee.OT1Amount = totalOT1 * Convert.ToDecimal(0.005138);
                    employee.OT2Amount = (employee.Gross != null ? employee.Gross.Value : 0) * (employee.OthersAllowance != null ? employee.OthersAllowance.Value : 0) * employee.OT2 * Convert.ToDecimal(0.006164);
                    employee.OT3Amount = (employee.Gross != null ? employee.Gross.Value : 0) * (employee.OthersAllowance != null ? employee.OthersAllowance.Value : 0) * employee.OT3 * Convert.ToDecimal(0.006164);
                    employee.OT4Amount = (employee.Gross != null ? employee.Gross.Value : 0) * (employee.OthersAllowance != null ? employee.OthersAllowance.Value : 0) * employee.OT4 * Convert.ToDecimal(0.006164);
                    employee.OTTotalHours = (employee.OT1 != null ? employee.OT1.Value : 0) + (employee.OT2 != null ? employee.OT2.Value : 0) + (employee.OT3 != null ? employee.OT3.Value : 0) + (employee.OT4 != null ? employee.OT4.Value : 0);
                    employee.OTTotalAmount = (employee.OT1Amount != null ? employee.OT1Amount.Value : 0) + (employee.OT2Amount != null ? employee.OT2Amount.Value : 0) + (employee.OT3Amount != null ? employee.OT3Amount.Value : 0) + (employee.OT4Amount != null ? employee.OT4Amount.Value : 0);
                    employee.GrandTotal = (employee.OTTotalAmount != null ? employee.OTTotalAmount.Value : 0) + (employee.OthersAmount != null ? employee.OthersAmount.Value : 0) + (employee.BonusAmount != null ? employee.BonusAmount.Value : 0);
                    employee.Deduction = 0;
                    employee.AdvanceAmount = 0;
                    employee.NetAmount = 0;
                    employee.TotalRecords = totalNumberOfRecords;
                    viewModel.Add(employee);
                }
                
            }
            return viewModel;
            // var total = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            //return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            //{
            //    EmployeeCode = x.EmployeeCode,
            //    Name = x.Name,
            //    Date = x.Date,
            //    TimeIn = x.TimeIn,
            //    TimeOut = x.TimeOut,
            //    Status = x.Status,
            //    OT1 = x.OT1 != null ? x.OT1.Value : 0,
            //    OT2 = x.OT2 != null ? x.OT2.Value : 0,
            //    OT3 = x.OT3 != null ? x.OT3.Value : 0,
            //    OT4 = x.OT4 != null ? x.OT4.Value : 0,
            //    TotalOT = (x.OT1 != null ? x.OT1.Value : 0 + x.OT2 != null ? x.OT2.Value : 0
            //                + x.OT3 != null ? x.OT3.Value : 0 + x.OT4 != null ? x.OT4.Value : 0),
            //});

        }
    }
}
