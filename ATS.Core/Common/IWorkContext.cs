using ATS.Core.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Common
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {

        /// <summary>
        ///     Gets or sets the current UserId
        /// </summary>
        Int64 UserId { get; set; }

        /// <summary>
        ///     Gets or sets the current Role
        /// </summary>
        ATSRole Role { get; set; }


        /// <summary>
        /// Gets  the current customer
        /// </summary>
       // Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Gets  the current Employee
        /// </summary>
        Employee CurrentEmployee { get; set; }


        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        string MapPath(string path);



        string RootPath();
    }
}
