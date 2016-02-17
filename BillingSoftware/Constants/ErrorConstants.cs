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
        internal static object ADMIN_NOT_LOGGED_IN = "Admin not logged in";
        internal static string ADMIN_USERNAME_ALREADY_TAKEN = "Admin user name already taken. Please choose different user name";
        internal static object INVALID_USERNAME_OR_PASSWORD = "Invalid user name or password";
        internal static object PROBLEM_CREATING_ADMIN = "Problem in creating admin. Please try again";
        internal static string PROBLEM_LOGOUT = "Problem Logout";
        internal static object REQUIRED_FIELD_EMPTY = "Required fields empty";
        internal static object SOMEBODY_LOGGEDIN = "Somebody already logged in";
        internal static string WRONG_PASSWORD = "Invalid password. Please try again ";
    }
}