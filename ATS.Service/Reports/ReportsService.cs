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

        public PagedResults<AttendanceViewModel> EmployeePayrollReport(GridRequestModel request)
        {
            var Employees = _employeeRepository.GetQueryable();
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
                EmployeeCode = x.EmployeeCode,
                Name = x.Name,
                Date = x.Date,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                Status = x.Status,
                OT1 = x.OT1 != null ? x.OT1.Value : 0,
                OT2 = x.OT2 != null ? x.OT2.Value : 0,
                OT3 = x.OT3 != null ? x.OT3.Value : 0,
                OT4 = x.OT4 != null ? x.OT4.Value : 0,
                TotalOT = (x.OT1 != null ? x.OT1.Value : 0 + x.OT2 != null ? x.OT2.Value : 0
                            + x.OT3 != null ? x.OT3.Value : 0 + x.OT4 != null ? x.OT4.Value : 0),
            });

        }
    }
}
