using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Constants
{
    public class ErrorConstants
    {
        public const string INDEX_CREATE_FAILED = "Index cannot be created";
        public const string LOGIN_FAILED = "Login failed. Please try again";
        internal static object INVALID_USERNAME_OR_PASSWORD = "Invalid user name or password";
        internal static string PROBLEM_LOGOUT = "Problem Logout";
        internal static object REQUIRED_FIELD_EMPTY = "Required fields empty";
        internal static object SOMEBODY_LOGGEDIN = "Somebody already logged in";
    }
}