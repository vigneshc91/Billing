using BillingSoftware.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class Employee
    {
        [ElasticProperty(Name = ConstEmployee.ID, Index = FieldIndexOption.NotAnalyzed)]
        public string employeeid { get; set; }

        [ElasticProperty(Name = ConstEmployee.NAME, Index = FieldIndexOption.Analyzed)]
        public string name { get; set; }

        [ElasticProperty(Name = ConstEmployee.ADDR_1, Index = FieldIndexOption.NotAnalyzed)]
        public string addr1 { get; set; }

        [ElasticProperty(Name = ConstEmployee.ADDR_2, Index = FieldIndexOption.NotAnalyzed)]
        public string addr2 { get; set; }

        [ElasticProperty(Name = ConstEmployee.CITY, Index = FieldIndexOption.NotAnalyzed)]
        public string city { get; set; }

        [ElasticProperty(Name = ConstEmployee.DISTRICT, Index = FieldIndexOption.NotAnalyzed)]
        public string district { get; set; }

        [ElasticProperty(Name = ConstEmployee.STATE, Index = FieldIndexOption.NotAnalyzed)]
        public string state { get; set; }

        [ElasticProperty(Name = ConstEmployee.COUNTRY, Index = FieldIndexOption.NotAnalyzed)]
        public string country { get; set; }

        [ElasticProperty(Name = ConstEmployee.PIN_CODE, Index = FieldIndexOption.NotAnalyzed)]
        public string pincode { get; set; }

        [ElasticProperty(Name = ConstEmployee.PHONE, Index = FieldIndexOption.NotAnalyzed)]
        public string phone { get; set; }

        [ElasticProperty(Name = ConstEmployee.EMAIL, Index = FieldIndexOption.NotAnalyzed)]
        public string email { get; set; }

        [ElasticProperty(Name = ConstEmployee.DESIGNATION, Index = FieldIndexOption.NotAnalyzed)]
        public string designation { get; set; }

        [ElasticProperty(Name = ConstEmployee.CREATED_AT, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }
    }
}