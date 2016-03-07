using BillingSoftware.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{

    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class User
    {
        [ElasticProperty(Name = ConstUser.ID, Index = FieldIndexOption.NotAnalyzed)]
        public string userid { get; set; }

        [ElasticProperty(Name = ConstUser.NAME, Index = FieldIndexOption.Analyzed)]
        public string name { get; set; }

        [ElasticProperty(Name = ConstUser.ADDR_1, Index = FieldIndexOption.NotAnalyzed)]
        public string addr1 { get; set; }

        [ElasticProperty(Name = ConstUser.ADDR_2, Index = FieldIndexOption.NotAnalyzed)]
        public string addr2 { get; set; }

        [ElasticProperty(Name = ConstUser.CITY, Index = FieldIndexOption.NotAnalyzed)]
        public string city { get; set; }

        [ElasticProperty(Name = ConstUser.DISTRICT, Index = FieldIndexOption.NotAnalyzed)]
        public string district { get; set; }

        [ElasticProperty(Name = ConstUser.STATE, Index = FieldIndexOption.NotAnalyzed)]
        public string state { get; set; }

        [ElasticProperty(Name = ConstUser.COUNTRY, Index = FieldIndexOption.NotAnalyzed)]
        public string country { get; set; }

        [ElasticProperty(Name = ConstUser.PIN_CODE, Index = FieldIndexOption.NotAnalyzed)]
        public string pincode { get; set; }

        [ElasticProperty(Name = ConstUser.PHONE, Index = FieldIndexOption.NotAnalyzed)]
        public string phone { get; set; }

        [ElasticProperty(Name = ConstUser.EMAIL, Index = FieldIndexOption.NotAnalyzed)]
        public string email { get; set; }

        [ElasticProperty(Name = ConstUser.CREATED_AT, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }

    }
}