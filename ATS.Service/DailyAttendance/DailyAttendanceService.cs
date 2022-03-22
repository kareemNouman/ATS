using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;
using ATS.Data;
using ATS.Service.Messages;

namespace ATS.Service.DailyAttendance
{
    public class DailyAttendanceService:BaseService, IDailyAttendanceService
    {
        private readonly IGenericRepository<Attendance> _attendanceRepository;
        private readonly IUnitOfWork _unitOfWrk;

        private readonly INotify _notify;
        public DailyAttendanceService(IGenericRepository<Attendance> attendanceRepository,
            IUnitOfWork unitOfWrk, INotify notify)
        {
            this._attendanceRepository = attendanceRepository;
            this._unitOfWrk = unitOfWrk;
            _notify = notify;
        }

        public IEnumerable<AttendanceViewModel> GetAllAttendance()
        {
            return _attendanceRepository.GetWithInclude(x => x.IsActive == true).Select(x => new AttendanceViewModel
            {
                Id = x.ID,
                EmployeeCode = x.EmployeeCode,
                Name = x.Name,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                TotalHours = x.TotalHours,
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3 = x.OT3,
                OT4 = x.OT4,
                Date = x.Date,
                Status = x.Status
            });
        }

        public ATSGridResponseModel<AttendanceViewModel> GetAllAttendance(int skip, int take)
        {
            ATSGridResponseModel<AttendanceViewModel> res = new ATSGridResponseModel<AttendanceViewModel>();

            var attendances = _attendanceRepository.GetQueryable().Where(x => x.IsActive == true);

            if (attendances != null)
            {
                res.Data = attendances.Select(x => new AttendanceViewModel
                {
                    Id = x.ID,
                    EmployeeCode = x.EmployeeCode,
                    Name = x.Name,
                    TimeIn = x.TimeIn,
                    TimeOut = x.TimeOut,
                    TotalHours = x.TotalHours,
                    OT1 = x.OT1,
                    OT2 = x.OT2,
                    OT3 = x.OT3,
                    OT4 = x.OT4,
                    Date = x.Date,
                    Status = x.Status
                }).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();

                res.Count = attendances.Count();
            }
            return res;
        }
        public bool AddAttendance(AttendanceViewModel attendanceviewmodel)
        {
            var result = false;
            //var validator = _validatorFactory.GetValidator<EmployeeViewModel>();
            //var res = validator.Validate(employeeviewmodel, ruleSet: "Add");

            //if (!res.IsValid)
            //{
            //    foreach (var item in res.Errors)
            //        _notify.AddMessage(item.ErrorMessage);
            //    return false;
            //}       


            var attendance = new Attendance
            {
                EmployeeCode = attendanceviewmodel.EmployeeCode,
                Name = attendanceviewmodel.Name,
                Designation = attendanceviewmodel.Designation,
                TimeIn = attendanceviewmodel.TimeIn,
                TimeOut = attendanceviewmodel.TimeOut,
                TotalHours = attendanceviewmodel.TotalHours,
                OT1 = attendanceviewmodel.OT1,
                OT2 = attendanceviewmodel.OT2,
                OT3 = attendanceviewmodel.OT3,
                OT4 = attendanceviewmodel.OT4,
                Date = attendanceviewmodel.Date,
                Status = attendanceviewmodel.Status,
                CreatedBy = attendanceviewmodel.CreatedBy,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _attendanceRepository.Insert(attendance);
            _unitOfWrk.Save();
            result = true;
            return result;
        }

        public Attendance GetAttendance(long id)
        {
            return _attendanceRepository.FirstOrDefault(x => x.ID == id && x.IsActive == true);
        }

        public void UpdateAttendance(Attendance attendance)
        {
            _attendanceRepository.Update(attendance);
            _unitOfWrk.Save();
        }

        public AttendanceViewModel GetAttendanceByID(long id)
        {

            return _attendanceRepository.GetWithInclude(x => x.ID == id && x.IsActive == true).Select(x => new AttendanceViewModel
            {
                Id = x.ID,
                Name = x.Name,
                EmployeeCode = x.EmployeeCode,
                Designation = x.Designation,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                TotalHours = x.TotalHours,
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3 = x.OT3,
                OT4 = x.OT4,
                Date = x.Date,
                Status = x.Status,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn
            }).FirstOrDefault();
        }

        public PayToAttendanceViewModel GetPayToByEmpCode(long empCode)
        {

            var attendancePayToData = _attendanceRepository.GetWithInclude(x => x.EmployeeCode == empCode && x.IsActive == true).Select(x => new AttendanceViewModel
            {                
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3 = x.OT3,
                OT4 = x.OT4,                
                Status = x.Status,
            }).ToList();

            PayToAttendanceViewModel payToAttendanceViewModel = new PayToAttendanceViewModel();
            payToAttendanceViewModel.TotalPayToDays = attendancePayToData.Where(x => x.Status == "").Count();
            payToAttendanceViewModel.TotalOT1 = attendancePayToData.Sum(x => x.OT1);
            payToAttendanceViewModel.TotalOT2 = attendancePayToData.Sum(x => x.OT2);
            payToAttendanceViewModel.TotalOT3 = attendancePayToData.Sum(x => x.OT3);
            payToAttendanceViewModel.TotalOT4 = attendancePayToData.Sum(x => x.OT4);
            return payToAttendanceViewModel;
        }

        public PagedResults<AttendanceViewModel> AttendanceDataRequest(GridRequestModel request)
        {

            var query = _attendanceRepository.ReadOnly();
            //var startdate = "";
            //var enddate = "";
            //var departmentid = "";
            //var employeecode = "";
            var employeename = "";

            //request.Filters.TryGetValue("startdate", out startdate);
            //request.Filters.TryGetValue("enddate", out enddate);
            //request.Filters.TryGetValue("departmentid", out departmentid);
            //request.Filters.TryGetValue("employeecode", out employeecode);
            request.Filters.TryGetValue("employeename", out employeename);

            //DateTime startDate;
            //DateTime endDate;
            //long departmentID;
            long employeeCode;

            //if (DateTime.TryParse(startdate, out startDate) && DateTime.TryParse(enddate, out endDate))
            //{
            //    if (startdate == enddate)
            //    {
            //        query = query.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(endDate));
            //    }
            //    else
            //        query = query.Where(x => DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(endDate));
            //}

            //if (long.TryParse(departmentid, out departmentID))
            //    query = query.Where(x => x.Designation == departmentid);

            //if (long.TryParse(employeecode, out employeeCode))
            //    query = query.Where(x => x.EmployeeCode == employeeCode);

            if (!string.IsNullOrWhiteSpace(employeename))
            {
                query = query.Where(x => x.Name.ToLower() == employeename.ToLower());
            }

            var total = query.Sum(x => x.OT1 + x.OT2 + x.OT3 + x.OT4);
            return PagedResults<Attendance, AttendanceViewModel>(query, request.Page.Value, request.PageSize, "ID", false, x => new AttendanceViewModel
            {
                Id =x.ID,
                Date = x.Date,
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
                Status = x.Status,
                OT1 = x.OT1,
                OT2 = x.OT2,
                OT3 = x.OT3,
                OT4 = x.OT4,
                TotalOT = x.OT1 + x.OT2 + x.OT3 + x.OT4,
                Total = total
            });

        }

        public bool Delete(long ID)
        {
            bool result = false;
            var attendance = _attendanceRepository.GetWithInclude(x => x.ID == ID && x.IsActive == true).FirstOrDefault();
            if (attendance != null)
            {
                attendance.IsActive = false;
                _attendanceRepository.Update(attendance);
                _unitOfWrk.Save();
                result = true;
            }
            return result;
        }
    }
}
