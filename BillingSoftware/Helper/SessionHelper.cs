using BillingSoftware.Constants;
using BillingSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Helper
{
    public class SessionHelper
    {

        public static string SetSession<T>(AdminSession<T> session) where T : class
        {
            if (session == null || String.IsNullOrWhiteSpace(session.id)) throw new Exception(ErrorConstants.LOGIN_FAILED);

            HttpContext.Current.Session["UserProfile"] = session;

            return session.id;
        }

        public static void RemoveSession<T>()where T : class
        {
            HttpContext.Current.Session.Remove("UserProfile");
        }

        public static Admin GetLoggedInAdmin(string token)
        {
            if (String.IsNullOrEmpty(token)) return null;
            var admin = (AdminSession<Admin>) HttpContext.Current.Session["UserProfile"];
            return admin.user;
        }

        }
    }