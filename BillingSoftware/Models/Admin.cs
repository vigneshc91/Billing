using BillingSoftware.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class Admin
    {
        [ElasticProperty(Name =ConstAdmin.ID, Index =FieldIndexOption.NotAnalyzed)]
        public Guid id { get; set; }

        [ElasticProperty(Name =ConstAdmin.USER_NAME, Index =FieldIndexOption.NotAnalyzed)]
        public string username { get; set; }

        [ElasticProperty(Name =ConstAdmin.PASSWORD, Index =FieldIndexOption.NotAnalyzed)]
        public string password { get; set; }

        [ElasticProperty(Name =ConstAdmin.SALT, Index =FieldIndexOption.NotAnalyzed)]
        public string salt { get; set; }

        [ElasticProperty(Name =ConstAdmin.TYPE, Index =FieldIndexOption.NotAnalyzed)]
        public Int16 type { get; set; }

        [ElasticProperty(Name =ConstAdmin.CREATED_AT, Index =FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }
    }
}