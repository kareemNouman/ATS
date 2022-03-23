using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;
using ATS.Data;

namespace ATS.Service.Reports
{
   public interface IReportsService
    {
        PagedResults<AttendanceViewModel> SingleEmployeeAttendanceReport(GridRequestModel request);
        PagedResults<AttendanceViewModel> AllEmployeeAttendanceReport(GridRequestModel request);
        PagedResults<AttendanceViewModel> OverTimeEmployeeAttendanceReport(GridRequestModel request);
    }
}
