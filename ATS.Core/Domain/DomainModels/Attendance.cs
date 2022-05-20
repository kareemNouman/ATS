using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DomainModels
{
    public class Attendance : Entity<Int64>
    {
        public Nullable<long> EmployeeCode { get; set; }

        public string Name { get; set; }

        public string Designation { get; set; }
        public string Department { get; set; }
        public string TimeIn { get; set; }

        public string ShiftCode { get; set; }
        public string TimeOut { get; set; }

        public double TotalHours { get; set; }
        public string Remarks { get; set; }
             
        public Nullable<System.DateTime> Date { get; set; }
 
        public Nullable<decimal> OT1 { get; set; }
        public Nullable<decimal> OT2 { get; set; }
        public Nullable<decimal> OT3 { get; set; }
        public Nullable<decimal> OT4 { get; set; }

        public string Status { get; set; }
        public bool? IsActive { get; set; }
        [IgnoreAudit]
        public Nullable<DateTime> CreatedOn { get; set; }
        [IgnoreAudit]
        public Nullable<long> CreatedBy { get; set; }
    }
}
