using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class AttendanceViewModel
    {
        public Int64 Id { get; set; }
        public Nullable<long> EmployeeCode { get; set; }

        public string Name { get; set; }

        public string Designation { get; set; }

        public string TimeIn { get; set; }

        public string TimeOut { get; set; }

        public double TotalHours { get; set; }
        public Nullable<decimal> SickLeaves { get; set; }
        public Nullable<decimal> AdjustAnnLeaves { get; set; }

        public Nullable<decimal> Absent { get; set; }

        public Nullable<decimal> ToPay { get; set; }
        public Nullable<decimal> OT1 { get; set; }
        public Nullable<decimal> OT2 { get; set; }
        public Nullable<decimal> OT3 { get; set; }
        public Nullable<decimal> OT4 { get; set; }

        public Nullable<decimal> TotalOT { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }

        public Nullable<long> CreatedBy { get; set; }

        public PayToAttendanceViewModel PayToAttendance { get; set; }
    }
}
