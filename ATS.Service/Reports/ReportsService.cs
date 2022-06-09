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
        private readonly IGenericRepository<EmployeeLeave> _employeeLeaveRepository;
        private readonly IGenericRepository<Designation> _designationRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Leaves> _leaveTypeRepository;
        private readonly IDailyAttendanceService _dailyAttendanceService;

        public ReportsService(IGenericRepository<Attendance> attendanceRepository, IGenericRepository<Leaves> leaveTypeRepository,
            IGenericRepository<Employee> employeeRepository, IGenericRepository<Department> departmentRepository,
        IDailyAttendanceService dailyAttendanceService, IGenericRepository<Designation> designationRepository,
          IGenericRepository<EmployeeLeave> employeeLeaveRepository)
        {
            this._attendanceRepository = attendanceRepository;
            this._dailyAttendanceService = dailyAttendanceService;
            this._employeeRepository = employeeRepository;
            this._employeeLeaveRepository = employeeLeaveRepository;
            this._designationRepository = designationRepository;
            this._departmentRepository = departmentRepository;
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public PagedResults<AttendanceViewModel> SingleEmployeeAttendanceReport(GridRequestModel request)
        {

            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            var departmentname = "";
            var employeecode = "";
            var employeename = "";
            var noemployeename = "";

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            request.Filters.TryGetValue("departmentname", out departmentname);
            request.Filters.TryGetValue("employeecode", out employeecode);
            request.Filters.TryGetValue("employeename", out employeename);
            request.Filters.TryGetValue("noemployeename", out noemployeename);            
            DateTime startDate;
            DateTime endDate;
            //long departmentID;            
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

            if (!string.IsNullOrWhiteSpace(departmentname))
                query = query.Where(x => x.Designation.ToLower() == departmentname.ToLower());

            if (long.TryParse(employeecode, out employeeCode))
                query = query.Where(x => x.EmployeeCode == employeeCode);

            if (!string.IsNullOrWhiteSpace(employeename))
            {
                query = query.Where(x => x.Name.ToLower() == employeename.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(noemployeename))
            {
                query = query.Where(x => x.Name.ToLower() == noemployeename.ToLower());
            }
            //query = query.OrderBy(x => x.Date);
            var isTotalHrExits = query.Sum(x => (double?)x.TotalHours);
            var totalHrs = isTotalHrExits != null ? query.Sum(x => x.TotalHours) : 0;
            var totalOTHrs = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            var totalOT1 = query.Sum(x => x.OT1);
            var totalOT2 = query.Sum(x => x.OT2);
            var totalOT3 = query.Sum(x => x.OT3);
            var totalOT4 = query.Sum(x => x.OT4);
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "Date", true, x => new AttendanceViewModel
            {
                Date = x.Date,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                Status = x.Status,
                TotalHours = Math.Round(x.TotalHours,1),
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3= x.OT3,
                OT4 =x.OT4,
                //TotalOT = Convert.ToDecimal(Math.Round(x.TotalHours, 1)) + x.OT1 + x.OT2 + x.OT3 + x.OT4,
                TotalOT =  x.OT1 + x.OT2 + x.OT3 + x.OT4,
                Total = totalOTHrs,
                TotalOT1 = totalOT1,
                TotalOT2 = totalOT2,
                TotalOT3 = totalOT3,
                TotalOT4 = totalOT4,
                SumTotalHrs = Math.Round(totalHrs,1),
                Remarks = x.Remarks,
                Name = x.Name,
                EmployeeCode = x.EmployeeCode,
                Department = x.Department,
                Designation = x.Designation
            });

        }

        public List<EmployeeViewModel> AllEmployeeAttendanceReport(GridRequestModel request)
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
            else
            {
                DateTime minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month -1, 20);
                DateTime maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 21);
                query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(minDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(maxDate));
            }
            var result = query.ToList();
            var employees = _employeeRepository.GetQueryable().OrderBy(x => x.EmployeeCode).Skip(request.Page.Value).Take(request.PageSize.Value).Where(x => x.IsActive == true).ToList();
            var totalNumberOfRecords = _employeeRepository.GetQueryable().Count();            
            List<EmployeeViewModel> viewModel = new List<EmployeeViewModel>();
            foreach (var item in employees)
            {
                EmployeeViewModel employee = new EmployeeViewModel();
                if (result.Where(x => x.EmployeeCode == item.EmployeeCode).Count() > 0)
                {
                    employee.Id = item.ID;                    
                    employee.EmployeeCode = item.EmployeeCode;
                    employee.Name = item.Name;                    
                    employee.Designation = _designationRepository.GetByID(item.DesignationID).Name;
                    employee.Department = _departmentRepository.GetByID(item.DepartmentID).Name;
                    employee.SickLeave = 0;
                    employee.AdjustAnnLeave = 0;
                    employee.Absent = result.Where(x => x.EmployeeCode == item.EmployeeCode && x.Remarks == "Absent").Count();
                    employee.ToPay = result.Where(x => x.EmployeeCode == item.EmployeeCode).Count() - employee.Absent;                 
                    employee.OT1 = item.OTThreshold > result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT1) ? result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT1) : item.OTThreshold;
                    employee.OT2 = 0;//result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT2);
                    employee.OT3 = 0;//result.Where(x => x.EmployeeCode == item.EmployeeCode).Sum(x => x.OT3);
                    employee.OT4 = 0;            
                    employee.OTTotalHours = (employee.OT1 != null ? employee.OT1.Value : 0) + (employee.OT2 != null ? employee.OT2.Value : 0) + (employee.OT3 != null ? employee.OT3.Value : 0) + (employee.OT4 != null ? employee.OT4.Value : 0);                                    
                    employee.TotalRecords = totalNumberOfRecords;
                    employee.Remarks = "";
                    employee.MonthDate = result.Select(x => x.Date).FirstOrDefault();
                    // employee.Month = result.Select(x => x.Date).FirstOrDefault().ToString("MMMM");
                    viewModel.Add(employee);
                }

            }
            return viewModel;
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

            //return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            //{
            //   // PayToAttendance = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value),
            //    EmployeeCode = x.EmployeeCode,
            //    Name = x.Name,
            //    Designation = x.Designation,
            //    SickLeaves = 1,
            //    AdjustAnnLeaves =0,
            //    Absent = 0,
            //    ToPay = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalPayToDays,
            //    OT1 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT1,
            //    OT2 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT2,
            //    OT3 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT3,
            //    OT4 = _dailyAttendanceService.GetPayToByEmpCode(x.EmployeeCode.Value).TotalOT4
            //    //TotalOT = 
            //});

        }
        public DateTime GetBillDate()
        {
            var minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 22);
            var billdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (DateTime.Now < minDate)
            {
                billdate = billdate.AddMonths(-1);
            }
            return billdate;
        }
        public PagedResults<AttendanceViewModel> OverTimeEmployeeAttendanceReport(GridRequestModel request)
        {

            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            var departmentname = "";          

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            request.Filters.TryGetValue("departmentname", out departmentname);          

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

            //if (long.TryParse(departmentid, out departmentID))
            //    query = query.Where(x => x.Department == departmentid);

            if (!string.IsNullOrWhiteSpace(departmentname))
                query = query.Where(x => x.Department.ToLower() == departmentname.ToLower());
           
            query = query.OrderByDescending(x => x.Date);
            var isTotalHrExits = query.Sum(x => (double?)x.TotalHours);
            var totalHrs = isTotalHrExits != null ? query.Sum(x => x.TotalHours) : 0;
            var totalOTHrs = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            var totalOT1 = query.Sum(x => x.OT1);
            var totalOT2 = query.Sum(x => x.OT1);
            var totalOT3 = query.Sum(x => x.OT1);
            var totalOT4 = query.Sum(x => x.OT1);
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            {
                Date = x.Date,
                EmployeeCode = x.EmployeeCode,
                Name =x.Name,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                Department = x.Department,
                Status = x.Status,
                TotalHours = Math.Round(x.TotalHours, 1),
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3 = x.OT3,
                OT4 = x.OT4,
                //TotalOT = Convert.ToDecimal(Math.Round(x.TotalHours, 1)) + x.OT1 + x.OT2 + x.OT3 + x.OT4,
                TotalOT = x.OT1 + x.OT2 + x.OT3 + x.OT4,
                Total = totalOTHrs,
                TotalOT1 = totalOT1,
                TotalOT2 = totalOT2,
                TotalOT3 = totalOT3,
                TotalOT4 = totalOT4,
                SumTotalHrs = Math.Round(totalHrs,1),
                Remarks = x.Remarks
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

        public PagedResults<EmployeeLeaveViewModel> EmployeeLeaveReport(GridRequestModel request)
        {

            var query = _employeeLeaveRepository.ReadOnly();
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
                    query = query.Where(x => DbFunctions.TruncateTime(x.CreatedOn) == DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.CreatedOn) == DbFunctions.TruncateTime(endDate));
                }
                else
                    query = query.Where(x => DbFunctions.TruncateTime(x.CreatedOn) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.CreatedOn) <= DbFunctions.TruncateTime(endDate));
            }

            //if (long.TryParse(departmentid, out departmentID))
            //    query = query.Where(x => x.Designation == departmentid);

            return PagedResults<EmployeeLeave, EmployeeLeaveViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new EmployeeLeaveViewModel
            {
                EmployeeCode = x.EmployeeCode,
                Name = x.Name,
                Department = x.DepartmentID != null ? _departmentRepository.GetByID(x.DepartmentID).Name : string.Empty,
                Designation = x.DesignationID != null ? _designationRepository.GetByID(x.DesignationID).Name : string.Empty,
                LeaveStart = x.LeaveStart,
                LeaveEnd = x.LeaveEnd,
                ExceedingDays = x.ExceedingDays,
                LName = x.LeaveType != null ? _leaveTypeRepository.GetByID(x.LeaveType).Name : string.Empty,
                Remark = x.Remark        
            });

        }
        //Overtime Employee Report Replica
        public PagedResults<AttendanceViewModel> DailyEmployeeAttendanceReport(GridRequestModel request)
        {
            var query = _attendanceRepository.ReadOnly();
            var startdate = "";
            var enddate = "";
            var departmentname = "";            
            var employeename = "";

            request.Filters.TryGetValue("startdate", out startdate);
            request.Filters.TryGetValue("enddate", out enddate);
            request.Filters.TryGetValue("departmentname", out departmentname);            
            request.Filters.TryGetValue("employeename", out employeename);

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

            if (!string.IsNullOrWhiteSpace(departmentname))
                query = query.Where(x => x.Department.ToLower() == departmentname.ToLower());

            if (!string.IsNullOrWhiteSpace(employeename))
            {
                query = query.Where(x => x.Name.ToLower() == employeename.ToLower());
            }

            //var total = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            var isTotalHrsNull = query.Sum(x =>(double?) x.TotalHours);
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            {
                EmployeeCode = x.EmployeeCode,
                Name = x.Name,
                Date = x.Date,
                Designation = _designationRepository.GetByID(_employeeRepository.FirstOrDefault(y => y.EmployeeCode == x.EmployeeCode).DesignationID).Name,
                Department = _departmentRepository.GetByID(_employeeRepository.FirstOrDefault(y => y.EmployeeCode == x.EmployeeCode).DepartmentID).Name,
                TimeIn = x.TimeIn,
                TimeOut = !string.IsNullOrEmpty(x.TimeOut) ? x.OT1 > 2 ? Convert.ToString(Convert.ToDecimal(x.TimeOut.Replace(':', '.')) - x.OT1 + 2) : x.TimeOut : "",                               
                TotalHours = isTotalHrsNull != null ? Math.Round(x.TotalHours, 1) : 0,
                Status = x.Status.ToUpper() != "L" ? x.Status : "",
                //Added need to add in server as well.
                Remarks = x.Remarks,
                OT1 = x.OT1 > 2 ? 2 : x.OT1,
                OT2 = 0,
                //OT2 = x.Status.ToUpper() == "WO" && x.TotalHours != 0 && x.TotalHours >= 4  ? x.OT2 = 4 : x.OT4,
                OT3 = x.OT3,
                OT4 = x.OT4,
                //TotalOT =  Convert.ToDecimal(Math.Round(x.TotalHours, 1)) + x.OT1 + x.OT2 + x.OT3 + x.OT4,
                //Total = total,
            });

        }

        public List<EmployeeViewModel> CashIncentivesReport(GridRequestModel request)
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
            else
            {
                DateTime minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 20);
                DateTime maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 21);
                query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(minDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(maxDate));
            }
            var result = query.ToList();
            var employees = _employeeRepository.GetQueryable().OrderBy(x => x.EmployeeCode).Skip(request.Page.Value).Take(request.PageSize.Value).Where(x => x.IsActive == true).ToList();
            var totalNumberOfRecords = _employeeRepository.GetQueryable().Count();
            List<EmployeeViewModel> viewModel = new List<EmployeeViewModel>();
            foreach (var item in employees)
            {
                EmployeeViewModel employee = new EmployeeViewModel();
                if (result.Where(x => x.EmployeeCode == item.EmployeeCode).Count() > 0)
                {
                    employee.Id = item.ID;
                    employee.EmployeeCode = item.EmployeeCode;
                    employee.Name = item.Name;
                    employee.Designation = _designationRepository.GetByID(item.DesignationID).Name;
                   // employee.Department = _departmentRepository.GetByID(item.DepartmentID).Name;                   
                    //employee.ToPay = result.Where(x => x.EmployeeCode == item.EmployeeCode).Count();
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
                    employee.TotalRecords = totalNumberOfRecords;
                    employee.Remarks = result.Select(x => x.Remarks).FirstOrDefault();
                    viewModel.Add(employee);
                }

            }
            return viewModel;          
        }

        public List<EmployeeViewModel> GeneratePayslipsReport(GridRequestModel request)
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
            else
            {
                DateTime minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 20);
                DateTime maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 21);
                query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(minDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(maxDate));
            }
            var result = query.ToList();
            var employees = _employeeRepository.GetQueryable().OrderBy(x => x.EmployeeCode).Skip(request.Page.Value).Take(request.PageSize.Value).Where(x => x.IsActive == true).ToList();
            var totalNumberOfRecords = _employeeRepository.GetQueryable().Count();
            List<EmployeeViewModel> viewModel = new List<EmployeeViewModel>();
            foreach (var item in employees)
            {
                EmployeeViewModel employee = new EmployeeViewModel();
                if (result.Where(x => x.EmployeeCode == item.EmployeeCode).Count() > 0)
                {
                    employee.Id = item.ID;
                    employee.EmployeeCode = item.EmployeeCode;
                    employee.Name = item.Name;
                    employee.Designation = _designationRepository.GetByID(item.DesignationID).Name;
                    // employee.Department = _departmentRepository.GetByID(item.DepartmentID).Name;                   
                    //employee.ToPay = result.Where(x => x.EmployeeCode == item.EmployeeCode).Count();
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
                    employee.TotalRecords = totalNumberOfRecords;
                    employee.Remarks = result.Select(x => x.Remarks).FirstOrDefault();
                    viewModel.Add(employee);
                }

            }
            return viewModel;
        }
    }
}
