using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ATS.Service;
using ATS.Service.Masters;
using ATS.Service.Messages;
using Microsoft.Owin.Security;

namespace ATS.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IMasterService _masterService;
        private readonly IAuthService _authService;
        private readonly INotify _notify;
        public AccountController(IMasterService masterService, IAuthService authService, INotify notify)
        {
            _masterService = masterService;
            _authService = authService;
            _notify = notify;
        }
        // GET: Account
        public ActionResult Index()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;

            Session.Clear();

            authManager.SignOut("ATS");
            return View();           
        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {

            var userinfo = _authService.Authenticate(email, password);

            if (userinfo != null && userinfo.ID > 0)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, email));
                //claims.Add(new Claim(ClaimTypes.NameIdentifier, userinfo["ID"]));
                //claims.Add(new Claim("token", userinfo["access_token"]));
                //claims.Add(new Claim("Role", userinfo.));

                var id = new ClaimsIdentity(claims, "ATS");

                var owinContext = Request.GetOwinContext();
                var authManager = owinContext.Authentication;
                authManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, id);


                ATSRole role = (ATSRole)Convert.ToInt64(userinfo.RoleID);

                //var customerID = Convert.ToInt64(userinfo["ID"]);
                //if (role == ATSRole.Customer)
                //{
                //    return RedirectToAction("Detail", "Customer", new { id = customerID });

                //}
                //else if (role == ATSRole.Cashier)
                //{
                //    return RedirectToAction("Index", "Customer");
                //}
                //else if (role != ATSRole.Admin)
                //{
                //    return RedirectToAction("Index", "Customer");
                //}

                return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.errorMessage = "Username or Password is Incorrect";

            return View();
        }

    }
}