using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATS.Web.Models
{
    public class PublicHolidaysViewModel
    {
        public Int64 ID { get; set; }
        public string Name { get; set; }

        public string Date { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}