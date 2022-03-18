using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATS.Web.Infrastructure.Enums;

namespace ATS.Web.Models
{
    public class PermissionViewModel
    {
        public Int64 RoleID { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public Nullable<MenuTypeEnum> MenuType { get; set; }
        public bool Checked { get; set; }
    }
}