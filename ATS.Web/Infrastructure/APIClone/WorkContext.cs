using Autofac;
using ATS.Core.Common;
using ATS.Core.Domain.DomainModels;
using ATS.Service.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ATS.Web.Infrastructure.APIClone
{
    public class WorkContext : IWorkContext
    {

        private readonly ILifetimeScope _lifetimeScope;


        public WorkContext(ILifetimeScope LifetimeScope)
        {
            _lifetimeScope = LifetimeScope;
        }


        //public Customer CurrentCustomer
        //{
        //    get
        //    {
        //        if (Role == NECRole.Customer)
        //        {
        //            var _customerService = _lifetimeScope.Resolve(typeof(ICustomerService)) as ICustomerService;
        //            return _customerService.GetCustomerByID(UserId);
        //        }
        //        return null;
        //    }
        //    set
        //    {

        //    }
        //}

        public Employee CurrentEmployee
        {
            get
            {
                if (Role != ATSRole.Customer)
                {
                    var _employerService = _lifetimeScope.Resolve(typeof(IEmployeeService)) as IEmployeeService;
                    return _employerService.GetEmployee(UserId);
                }
                return null;
            }

            set { }
        }

        public ATSRole Role
        {
            get
            {
                return HttpContext.Current != null ? HttpContext.Current.User.Identity.GetRole() : ATSRole.Empty;

            }

            set { }
        }


        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            //hosted
            return HostingEnvironment.MapPath(path);
        }

        public virtual string RootPath()
        {
            return HostingEnvironment.MapPath("~/");
        }


        public long UserId
        {
            get
            {
                return HttpContext.Current != null ? HttpContext.Current.User.Identity.GetUserID() : 0;
            }

            set
            {

            }
        }
    }
}