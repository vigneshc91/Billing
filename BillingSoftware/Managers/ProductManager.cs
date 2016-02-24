using BillingSoftware.Constants;
using BillingSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class ProductManager: ElasticSearchManager
    {

        public bool AddProduct(Admin admin, Product product)
        {
            if (product == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (short)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var productExist = GetProductById(admin, product.productid);

                if (productExist != null)
                    throw new Exception(ErrorConstants.PRODUCT_WITH_GIVEN_ID_ALREADY_EXIST);

                var elasticClient = GetElasticClient();

                var response = elasticClient.Index<Product>(product, i => i
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_PRODUCT)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Product GetProductById(Admin admin, string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (short)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Product>(g => g
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_PRODUCT)
                .Filter(f => f.Term(ConstProduct.ID, id))
                .Size(1));

                Product product = null;

                if(response.Total > 0)
                    foreach (var item in response.Hits)
                        product = item.Source;
                    

                return product;

            }
            catch (Exception e)
            {

                throw e;
            }

        }

    }
}