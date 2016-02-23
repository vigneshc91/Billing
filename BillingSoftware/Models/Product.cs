using BillingSoftware.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class Product
    {
        [ElasticProperty(Name =ConstProduct.ID, Index =FieldIndexOption.NotAnalyzed)]
        public string id { get; set; }

        [ElasticProperty(Name =ConstProduct.PRODUCT_NAME, Index =FieldIndexOption.Analyzed)]
        public string productname { get; set; }

        [ElasticProperty(Name =ConstProduct.PRICE, Index =FieldIndexOption.NotAnalyzed)]
        public double price { get; set; }

        [ElasticProperty(Name =ConstProduct.QUANTITY, Index =FieldIndexOption.NotAnalyzed)]
        public float quantity { get; set; }

        [ElasticProperty(Name =ConstProduct.UNIT, Index =FieldIndexOption.NotAnalyzed)]
        public Int16 unit { get; set; }

        [ElasticProperty(Name =ConstProduct.CREATED_AT, Index =FieldIndexOption.NotAnalyzed)]
        public DateTime create_at { get; set; }
    }
}