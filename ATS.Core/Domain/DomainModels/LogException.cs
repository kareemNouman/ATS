using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DomainModels
{
    public class LogException : Entity<Int64>
    {
        public string ExceptionMessage { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
