using BillingSoftware.Constants;
using BillingSoftware.Helper;
using BillingSoftware.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillingSoftware.Controllers
{
    public class AdminController : Controller
    {

        AdminManager adminManager = new AdminManager();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoginAdmin()
        {
            var userName = Request.Params.Get(AppConstants.USER_NAME);
            var password = Request.Params.Get(AppConstants.PASSWORD);

            var response = new ServiceResponse();
            if(String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            var cookieToken = HttpContext.Request.Cookies.Get(AppConstants.ACCESS_TOKEN);
            if(cookieToken != null)
            {
                response.result = ErrorConstants.SOMEBODY_LOGGEDIN;
                return Json(response);
            }

            try
            {
                var admin = adminManager.LoginAdmin(userName, password);
                if (admin != null)
                {
                    response.result = CookieHelper.SetSession(HttpContext, admin);
                    response.status = response.result == null ? false : true;
                }
                else
                    response.result = ErrorConstants.INVALID_USERNAME_OR_PASSWORD;
            }
            catch (Exception e )
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return Json(response);
        }

        public ActionResult LogoutAdmin()
        {
            try
            {
                var admin = CookieHelper.GetLoggedInAdmin(HttpContext);

                var token = CookieHelper.GetCookie(HttpContext);

                if (String.IsNullOrWhiteSpace(token))
                {
                    CookieHelper.RemoveCookie(HttpContext);
                }

                var status = adminManager.LogoutAdmin(token);
                CookieHelper.RemoveCookie(HttpContext);
                if (status)
                    return Redirect("/Home");

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return View();
        }
    }
}