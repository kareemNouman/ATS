using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DomainModels
{
    public class Role : Entity<Int64>
    {
        //private ICollection<PermissionRecord> _permissionRecords;

        public Role()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public string Name { get; set; }
        public ATSRole RoleType { get; set; }      
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Gets or sets the permission records
        /// </summary>
        //public virtual ICollection<PermissionRecord> PermissionRecords
        //{
        //    get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
        //    protected set { _permissionRecords = value; }
        //}

    }
}
