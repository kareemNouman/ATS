using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class PayToAttendanceViewModel
    {
        public Nullable<decimal> TotalPayToDays { get; set; }
        public Nullable<decimal> TotalOT1 { get; set; }
        public Nullable<decimal> TotalOT2 { get; set; }
        public Nullable<decimal> TotalOT3 { get; set; }
        public Nullable<decimal> TotalOT4 { get; set; }

        public Nullable<decimal> TotalOT { get; set; }
    }
}
