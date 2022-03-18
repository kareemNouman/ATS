using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class EmployeeViewModel
    {
        public Int64 Id { get; set; }

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
        public int WeeklyOffAlternate { get; set; }

        public bool? IsOTEligible { get; set; }
        public Nullable<System.DateTime> DORJ { get; set; }

        public bool? IsActive { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }
        
        public Nullable<long> CreatedBy { get; set; }
    }
}
