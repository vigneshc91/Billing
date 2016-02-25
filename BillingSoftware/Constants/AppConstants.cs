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
        public const string ID = "id";
        public const string OLD_PASSWORD = "old_password";
        public const string NEW_PASSWORD = "new_password";

        public const string PRODUCT_ID = "product_id";
        public const string PRODUCT_NAME = "product_name";
        public const string PRICE = "price";
        public const string QUANTITY = "quantity";
        public const string UNIT = "unit";
        public const string START = "start";
        public const string SIZE = "size";
        public const int START_VALUE = 0;
        public const int SIZE_VALUE = 10;
    }
}