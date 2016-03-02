using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Constants
{
    public class ElasticMappingConstants
    {
        public const string INDEX_NAME = "billing";
        public const string TYPE_ADMIN = "admin";
        public const string TYPE_PRODUCT = "product";
        public const string TYPE_USER = "user";
        public const string TYPE_SALES = "sales";
        public const string TYPE_SALES_INFO = "sales_info";
        public const string ID = "id";
        public const string ELASTIC_SEARCH_URL = "http://127.0.0.1:9200/";
    }

    public class ConstAdmin
    {
        public const string ID = "id";
        public const string USER_NAME = "user_name";
        public const string PASSWORD = "password";
        public const string SALT = "salt";
        public const string TYPE = "type";
        public const string CREATED_AT = "created_at";
    }

    public class ConstProduct
    {
        public const string ID = "product_id";
        public const string PRODUCT_NAME = "product_name";
        public const string PRICE = "price";
        public const string QUANTITY = "quantity";
        public const string UNIT = "unit";
        public const string CREATED_AT = "created_at";
    }

    public class ConstUser
    {
        public const string ID = "user_id";
        public const string NAME = "name";
        public const string ADDR_1 = "address_1";
        public const string ADDR_2 = "address_2";
        public const string CITY = "city";
        public const string DISTRICT = "district";
        public const string STATE = "state";
        public const string COUNTRY = "country";
        public const string PIN_CODE = "pin_code";
        public const string PHONE = "phone";
        public const string EMAIL = "email";
        public const string CREATED_AT = "created_at";
    }

    public class ConstSales
    {
        public const string ID = "sales_id";
        public const string DATE_TIME = "date_time";
        public const string USER_ID = "user_id";
        public const string NAME = "name";
        public const string TAX = "tax";
        public const string DISCOUNT = "discount";
        public const string STATUS = "status";
        public const string CREATED_AT = "created_at";
    }

    public class ConstSalesInfo
    {
        public const string SALES_ID = "sales_id";
        public const string PRODUCT_ID = "product_id";
        public const string QUANTITY = "quantity";
        public const string PRICE = "price";
    }
}