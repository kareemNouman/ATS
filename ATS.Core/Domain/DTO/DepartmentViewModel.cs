using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class DepartmentViewModel
    {
        public Int64 ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
