using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Integration.Owin;
using Autofac;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using ATS.Service;

namespace ATS.Web.App_Start
{
    public class ATSAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public IAuthService _authService { get; set; }

        public ATSRole RoleID { get; set; }
        public Int64 ID { get; set; }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            try
            {
                var oath = context.OwinContext.GetAutofacLifetimeScope();
                using (var scope = oath.BeginLifetimeScope())
                {
                    var service = scope.Resolve<IAuthService>();

                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                    var account = service.Authenticate(context.UserName, context.Password);


                    if (account == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }

                    if (account.IsActive == false)
                    {
                        context.SetError("invalid_grant", "User is Inactive Please contact administrator.");
                        return;
                    }

                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("sub", context.UserName));
                    //identity.AddClaim(new Claim("role", Convert.ToInt32(account.RoleID).ToString()));
                    identity.AddClaim(new Claim("RoleID", Convert.ToString(account.RoleID)));

                    // RoleID = account.RoleID;

                    //if (RoleID == ATSRole.Customer)
                    //    ID = service.GetCustomerIDByAccountID(account.ID);
                    //else
                    //    ID = service.GetEmployeeIDByAccountID(account.ID);

                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(ID)));

                    context.Validated(identity);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            context.AdditionalResponseParameters.Add("Role", RoleID);
            context.AdditionalResponseParameters.Add("ID", ID);
            return base.TokenEndpointResponse(context);
        }
    }
}