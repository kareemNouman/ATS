using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DomainModels
{
    public class Leaves : Entity<Int64>
    {
        public string Name { get; set; }
        [IgnoreAudit]
        public Nullable<bool> IsDelete { get; set; }
    }
}
