using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ATS.Core.Common;
using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.DTO;
using ATS.Service.Messages;
using ATS.Service.Validators;
using ATS.Web.Infrastructure.APIClone;
using ATS.Web.Infrastructure.APIClone.Validation;
using Autofac;
using Autofac.Integration.Mvc;
using FluentValidation;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(ATS.Web.App_Start.Startup))]

namespace ATS.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            var builder = new ContainerBuilder();

            // Register your MVC controllers.            
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => t.Name.EndsWith("Service"))
               .AsSelf()
               .InstancePerRequest();

            var necData = Assembly.Load("ATS.Data");
            Type dbcontext = necData.GetTypes().Where(x => x.Name == "ATSDbContext").SingleOrDefault();
            Type unitofwork = necData.GetTypes().Where(x => x.Name == "UnitOfWork").SingleOrDefault();
            Type iunitofwork = necData.GetTypes().Where(x => x.Name == "IUnitOfWork").SingleOrDefault();
            Type repository = necData.GetTypes().Where(x => x.Name == "GenericRepository`1").SingleOrDefault();
            Type irepository = necData.GetTypes().Where(x => x.Name == "IGenericRepository`1").SingleOrDefault();

            builder.RegisterGeneric(repository).As(irepository).InstancePerRequest();
            builder.RegisterType(dbcontext).AsSelf().InstancePerRequest();
            builder.RegisterType(unitofwork).As(iunitofwork).InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.Load("ATS.Service"))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .InstancePerRequest();

            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerRequest();
            builder.RegisterType<Notify>().As<INotify>().InstancePerRequest();
            
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().InstancePerRequest();
           
            builder.RegisterType<DepartmentValidator>().As<IValidator<Department>>().InstancePerRequest();
            builder.RegisterType<LeavesValidator>().As<IValidator<Leaves>>().InstancePerRequest();
            builder.RegisterType<DesignationValidator>().As<IValidator<Designation>>().InstancePerRequest();
            builder.RegisterType<EmployeeValidation>().As<IValidator<EmployeeViewModel>>().InstancePerRequest();

            builder
             .RegisterType<ATSAuthorizationServerProvider>()
             .As<IOAuthAuthorizationServerProvider>()
             .PropertiesAutowired()
              .InstancePerRequest();
            var container = builder.Build();

            builder = new ContainerBuilder();
           
            builder.Update(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ATS",
                LoginPath = new PathString("/Account"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                ExpireTimeSpan = TimeSpan.FromDays(180),
            });

            app.UseAutofacMiddleware(container);
            ConfigureOAuth(app, container);
            app.UseAutofacMvc();
        }
        public void ConfigureOAuth(IAppBuilder app, IContainer container)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                //Provider = container.Resolve<IOAuthAuthorizationServerProvider>()
                Provider = new ATSAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

    }
}
