using BillingSoftware.Constants;
using BillingSoftware.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Helper
{
    public class ElasticSearchMappings
    {
        public void CheckMappings(ElasticClient client)
        {
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_ADMIN)).Exists)
            {
                if (!CreateAdmin(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_PRODUCT)).Exists)
            {
                if (!CreateProduct(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_USER)).Exists)
            {
                if (!CreateUser(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_EMPLOYEE)).Exists)
            {
                if (!CreateEmployee(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_SALES)).Exists)
            {
                if (!CreateSales(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_SALES_INFO)).Exists)
            {
                if (!CreateSalesInfo(client)) throw new Exception();
            }
        }

        private bool CreateAdmin(ElasticClient client)
        {
            var response = client.Map<Admin>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_ADMIN)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateProduct(ElasticClient client)
        {
            var response = client.Map<Product>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_PRODUCT)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateUser(ElasticClient client)
        {
            var response = client.Map<User>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_USER)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateEmployee(ElasticClient client)
        {
            var response = client.Map<Employee>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateSales(ElasticClient client)
        {
            var response = client.Map<Sales>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_SALES)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateSalesInfo(ElasticClient client)
        {
            var response = client.Map<SalesInfo>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_SALES_INFO)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }
    }
}