using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class UserAccountViewModel
    {
        public Int64 Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<Int64> RoleID { get; set; }
        public string RoleName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public Nullable<bool> IsActive { get; set; }
        [IgnoreAudit]
        public Nullable<long> CreatedBy { get; set; }
        [IgnoreAudit]
        public Nullable<System.DateTime> CreatedOn { get; set; }      
    }
}
