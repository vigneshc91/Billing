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
                    response.result = CookieHelper.SetSession(HttpContext, admin) ? SuccessConstants.LOGIN_SUCCESS: ErrorConstants.LOGIN_FAILED;
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

        public JsonResult CreateAdmin()
        {
            var userName = Request.Params.Get(AppConstants.USER_NAME);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(userName))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            try
            {
                if (adminManager.CreateAdmin(admin, userName))
                {
                    response.result = SuccessConstants.ADMIN_CREATED;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.PROBLEM_CREATING_ADMIN;

                return Json(response);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return Json(response);
        }

        public JsonResult GenerateAdminPassword()
        {
            var id = Request.Params.Get(AppConstants.ID);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            Guid userId;
            if(!Guid.TryParse(id, out userId))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }

            try
            {
                response.result = adminManager.GenerateAdminPassword(admin, userId);
                response.status = true;
                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return Json(response);
        }

        public JsonResult ChangePassword()
        {
            var id = Request.Params.Get(AppConstants.ID);
            var oldPassword = Request.Params.Get(AppConstants.OLD_PASSWORD);
            var newPassword = Request.Params.Get(AppConstants.NEW_PASSWORD);

            Guid userId;

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if(String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(oldPassword) || String.IsNullOrWhiteSpace(newPassword))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if(String.Equals(oldPassword, newPassword))
            {
                response.result = ErrorConstants.NO_CHANGES;
                return Json(response);
            }
            if(!Guid.TryParse(id, out userId))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            try
            {
                response.result =  adminManager.ChangePassword(admin, userId, oldPassword, newPassword) ? SuccessConstants.PASSWORD_UPDATED : ErrorConstants.PASSWORD_UPDATE_FAILED;
                response.status = true;

                return Json(response);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return Json(response);
        }

        public JsonResult DeleteAdmin()
        {
            var id = Request.Params.Get(AppConstants.ID);

            Guid adminId;
            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if(!Guid.TryParse(id, out adminId))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }

            try
            {
                response.result = adminManager.DeleteAdmin(admin, adminId) ? SuccessConstants.ADMIN_DELETED : ErrorConstants.PROBLEM_DELETING_AMDIN;
                response.status = true;
                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return Json(response);

        }
    }
}