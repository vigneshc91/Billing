using BillingSoftware.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class Sales
    {
        [ElasticProperty(Name = ConstSales.ID, Index = FieldIndexOption.NotAnalyzed)]
        public string salesid { get; set; }

        [ElasticProperty(Name = ConstSales.DATE_TIME, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime date { get; set; }

        [ElasticProperty(Name = ConstSales.USER_ID, Index = FieldIndexOption.NotAnalyzed)]
        public string userid { get; set; }

        [ElasticProperty(Name = ConstSales.USER_NAME, Index = FieldIndexOption.NotAnalyzed)]
        public string username { get; set; }

        [ElasticProperty(Name = ConstSales.TAX, Index = FieldIndexOption.NotAnalyzed)]
        public float tax { get; set; }

        [ElasticProperty(Name = ConstSales.DISCOUNT, Index = FieldIndexOption.NotAnalyzed)]
        public float discount { get; set; }

        [ElasticProperty(Name = ConstSales.STATUS, Index = FieldIndexOption.NotAnalyzed)]
        public Int16 status { get; set; }

        [ElasticProperty(Name = ConstSales.CREATED_AT, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }

        public virtual User user { get; set; }
    }
}