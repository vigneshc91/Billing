using BillingSoftware.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class SalesInfo
    {
        [ElasticProperty(Name = ConstSalesInfo.SALES_ID, Index = FieldIndexOption.NotAnalyzed)]
        public string salesid { get; set; }

        [ElasticProperty(Name = ConstSalesInfo.PRODUCT_ID, Index = FieldIndexOption.NotAnalyzed)]
        public string productid { get; set; }

        [ElasticProperty(Name = ConstSalesInfo.EMPLOYEE_ID, Index = FieldIndexOption.NotAnalyzed)]
        public string employeeid { get; set; }

        [ElasticProperty(Name = ConstSalesInfo.QUANTITY, Index = FieldIndexOption.NotAnalyzed)]
        public float quantity { get; set; }

        [ElasticProperty(Name = ConstSalesInfo.PRICE, Index = FieldIndexOption.NotAnalyzed)]
        public double price { get; set; }

        public virtual Sales sales { get; set; }
        public virtual Product product { get; set; }
        public virtual Employee employee { get; set; }
    }
}