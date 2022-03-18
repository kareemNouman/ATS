using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core
{

    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        public DateTime CreatedDate { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Int64 UpdatedBy { get; set; }
    }
}
