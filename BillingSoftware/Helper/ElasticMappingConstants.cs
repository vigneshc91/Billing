﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Helper
{
    public class ElasticMappingConstants
    {
        public const string INDEX_NAME = "billing";
        public const string TYPE_ADMIN = "admin";
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
}