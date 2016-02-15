using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Constants
{
    public class AppConstants
    {
        public const string SUPER_ADMIN_USER_NAME = "admin123";
        public const string SUPER_ADMIN_PASSWORD = "admin123";
        public const string SALT = "!2#4%6&8(0)9*7^5$3@1";
        public const int TOKEN_LENGTH = 7;
        public const int SESSION_ID_LENGTH = 26;
        public const string ACCESS_TOKEN = "AccessToken";
        public const string USER_NAME = "user_name";
        public const string PASSWORD = "password";

    }
}