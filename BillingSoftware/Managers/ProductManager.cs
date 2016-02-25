using BillingSoftware.Constants;
using BillingSoftware.Models;
using Nest;
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

        public bool UpdateProduct(Admin admin, Product product)
        {
            if (product == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (short)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var productElasticId = GetProductId(product.productid);
                if(String.IsNullOrWhiteSpace(productElasticId))
                {
                    throw new Exception(ErrorConstants.PRODUCT_NOT_FOUND);
                }

                var productUpdate = new Dictionary<string, object>();
                if (!String.IsNullOrWhiteSpace(product.productname))
                    productUpdate[ConstProduct.PRODUCT_NAME] = product.productname;
                if (product.price>0)
                    productUpdate[ConstProduct.PRICE] = product.price;
                if (product.quantity>0)
                    productUpdate[ConstProduct.QUANTITY] = product.quantity;
                if (product.unit>0)
                    productUpdate[ConstProduct.UNIT] = product.unit;

                var elasticClient = GetElasticClient();

                var response = elasticClient.Update<Product, object>(u => u
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_PRODUCT)
                .Id(productElasticId)
                .Doc(productUpdate)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public string GetProductId(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Product>(g => g
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_PRODUCT)
                .Filter(f => f.Term(ConstProduct.ID, id))
                .Size(1));

                if(response.Total > 0)
                    foreach (var item in response.Hits)
                        return item.Id;

                return null;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool DeleteProduct(Admin admin, string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (short)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var productElasticId = GetProductId(id);
                if (String.IsNullOrWhiteSpace(productElasticId))
                    throw new Exception(ErrorConstants.PRODUCT_NOT_FOUND);

                var elasticClient = GetElasticClient();

                var response = elasticClient.Delete<Product>(productElasticId, d => d
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

        public List<Product> GetProductList(Admin admin, int start, int size)
        {
            if (admin == null || admin.type != (short)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Product>(g => g
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_PRODUCT)
                .Skip(start)
                .Take(size)
                );

                List<Product> productList = new List<Product>();

                if(response.Total > 0)
                    foreach (var item in response.Hits)
                        productList.Add(item.Source);

                return productList;
            }
            catch (Exception e)
            {

                throw e;
            } 
        }

        public List<Product> GetProductByName(Admin admin, string name, int start, int size)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (short)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Product>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_PRODUCT)
                .Query(q => q.Term(ConstProduct.PRODUCT_NAME, name))
                .Skip(start)
                .Take(size)
                );

                List<Product> productList = new List<Product>();

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        productList.Add(item.Source);

                return productList;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}