using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Core.Domain.ResponseModels;

namespace ATS.Service.DailyAttendance
{
    public interface IDailyAttendanceService
    {
        IEnumerable<AttendanceViewModel> GetAllAttendance();

        bool AddAttendance(AttendanceViewModel attendanceviewmodel);

        ATSGridResponseModel<AttendanceViewModel> GetAllAttendance(int skip, int take);

        Attendance GetAttendance(Int64 id);


        AttendanceViewModel GetAttendanceByID(Int64 id);

        void UpdateAttendance(Attendance attendance);

        PayToAttendanceViewModel GetPayToByEmpCode(long empCode);

        bool Delete(long ID);
    }
}
