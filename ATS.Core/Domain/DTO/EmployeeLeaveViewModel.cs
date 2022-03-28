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
        public Nullable<long> DepartmentID { get; set; }
        public Nullable<long> DesignationID { get; set; }
        public string Name { get; set; }
        //Name of the Leave means type of leave
        public string LName { get; set; }
        public string Department { get; set; }

        public string Designation { get; set; }
        public Nullable<long> EmployeeId { get; set; }
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
