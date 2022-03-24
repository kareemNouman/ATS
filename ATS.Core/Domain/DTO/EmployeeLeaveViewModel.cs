using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class EmployeeLeaveViewModel
    {
        public Int64 Id { get; set; }
        public Nullable<long> EmployeeCode { get; set; }

        public string Name { get; set; }

        public Nullable<System.DateTime> LeaveStart { get; set; }

        public Nullable<System.DateTime> LeaveEnd { get; set; }
        public Nullable<System.DateTime> ExceedingDays { get; set; }

        public Nullable<Int64> LeaveType { get; set; }

        public string Remark { get; set; }

        public bool? IsActive { get; set; }

        public Nullable<long> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
