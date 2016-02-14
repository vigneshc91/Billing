using BillingSoftware.Constants;
using BillingSoftware.Models;
using BillingSoftware.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Helper
{
    public class CookieHelper
    {
        static SkyNet secure = new SkyNet(AppConstants.SALT);

        public static T PrepareUser<T>(T admin) where T : class
        {
            if (typeof(T) == typeof(Admin))
            {
                (admin as Admin).password = null;
                (admin as Admin).salt = null;
            }

            return admin;
        }

        public static bool SetSession<T>(HttpContextBase context, T admin) where T : class
        {
            if (admin == null || (typeof(T) != typeof(Admin))) return false;

            var session = new AdminSession<T>()
            {
                id = TokenHelper.GetUniqueKey(AppConstants.SESSION_ID_LENGTH),
                ipaddress = HttpContext.Current.Request.UserHostAddress,
                created_at = DateTime.UtcNow,
                user = PrepareUser<T>(admin),
                user_type = (short) BillingEnums.USER_TYPE.ADMIN
            };

            var token = SessionHelper.SetSession(session);
            SetCookie(context, token);

            return true;

        }

        public static Admin GetLoggedInAdmin(HttpContextBase context)
        {
            try
            {
                var token = GetCookie(context);
                if (String.IsNullOrWhiteSpace(token)) return null;
                Admin admin = SessionHelper.GetLoggedInAdmin(token);
                return admin;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return null;
            }
        }

        public static string GetCookie(HttpContextBase context)
        {
            var cookie = context.Request.Cookies.Get(AppConstants.ACCESS_TOKEN);
            if (cookie == null) return null;
            return secure.DecryptRijndael(cookie.Value);
        }

        public static void SetCookie(HttpContextBase context, string token)
        {
            if (String.IsNullOrWhiteSpace(token)) return;
            var encrypted = secure.EncryptRijndael(token);
            context.Response.SetCookie(new HttpCookie(AppConstants.ACCESS_TOKEN, encrypted));
        }

        public static void RemoveCookie(HttpContextBase context)
        {
            var cookie = context.Request.Cookies.Get(AppConstants.ACCESS_TOKEN);
            if (cookie == null) return;

            cookie.Expires = DateTime.UtcNow.AddDays(-25);
            context.Response.Cookies.Set(cookie);
        }

    }
}