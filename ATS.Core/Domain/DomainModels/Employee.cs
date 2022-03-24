using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core;

namespace ATS.Core.Domain.DomainModels
{
    public class Employee : Entity<Int64>
    {
        public Nullable<long> EmployeeCode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Nullable<long> DepartmentID { get; set; }

        public Nullable<long> DesignationID { get; set; }

        public Nullable<decimal> Basic { get; set; }

        public Nullable<decimal> SplAllowance { get; set; }
        public Nullable<decimal> Col { get; set; }
        public Nullable<decimal> OthersAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<decimal> Housing { get; set; }
        public Nullable<decimal> Gross { get; set; }

        public Nullable<decimal> OTThreshold { get; set; }
        public int WeekOffMain { get; set; }

        public int ShiftCode { get; set; }
        public int WeeklyOffAlternate { get; set; }

        public bool? IsOTEligible { get; set; }
        public Nullable<System.DateTime> DORJ { get; set; }
        
        public bool? IsActive { get; set; }

        [IgnoreAudit]
        public Nullable<long> CreatedBy { get; set; }
        [IgnoreAudit]
        public Nullable<System.DateTime> CreatedOn { get; set; }
       
    }

    public class DBAudit : Entity<Int64>
    {

        public string TableName { get; set; }

        public Int64 UserId { get; set; }
        public Int64 PrimaryKeyValue { get; set; }
        public string NewData { get; set; }
        public string OldData { get; set; }
        public AuditActions Actions { get; set; }

        public ATSRole Role { get; set; }
        public DateTime ActionDate { get; set; }

        //public string ChangedColumns { get; set; }
    }


    public enum AuditActions : int
    {
        Insert = 1,
        Update = 2,
        Delete = 3
    }
}
