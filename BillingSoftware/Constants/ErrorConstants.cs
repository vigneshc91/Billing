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
        public const string ADMIN_NOT_LOGGED_IN = "Admin not logged in";
        public const string ADMIN_USERNAME_ALREADY_TAKEN = "Admin user name already taken. Please choose different user name";
        public const string INVALID_ID = "Invalid id";
        public const string INVALID_USERNAME_OR_PASSWORD = "Invalid user name or password";
        public const string NO_PREVILAGE = "You don't have previlage to perform this operation";
        public const string PASSWORD_UPDATE_FAILED = "password change failed. Please try again";
        public const string PROBLEM_CREATING_ADMIN = "Problem in creating admin. Please try again";
        public const string PROBLEM_LOGOUT = "Problem Logout";
        public const string PROBLEM_OCCURED_WHILE_GENERATING_PASSWORD = "Problem occured while generating password. Please try again";
        public const string REQUIRED_FIELD_EMPTY = "Required fields empty";
        public const string SOMEBODY_LOGGEDIN = "Somebody already logged in";
        public const string WRONG_PASSWORD = "Invalid password. Please try again ";
        public const string ADMIN_NOT_FOUND = "Admin not found";
        public const string PROBLEM_DELETING_AMDIN = "Problem in deleting admin. Please try again";
        public const string NO_CHANGES = "No changes to made";
        public const string PROBLEM_OCCURES_ON_RETRIVING_ADMIN_LIST = "Problem occured on retriving admin list. Please try again";
        public const string INVALID_DATA = "Invalid data. Please check the value you entered";
        public const string PROBLEM_ADDING_PRODUCT = "Problem in adding product. Please try again";
        public static string PRODUCT_WITH_GIVEN_ID_ALREADY_EXIST;
    }
}