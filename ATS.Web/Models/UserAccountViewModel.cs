using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATS.Web.Models
{
    public class UserAccountViewModel
    {
        public Int64 Id { get; set; }
        public string UserName { get; set; }
        public Nullable<Int64> RoleID { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public Nullable<bool> IsActive { get; set; }
      
        public Nullable<long> CreatedBy { get; set; }
      
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}