using BillingSoftware.Constants;
using BillingSoftware.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class SalesManager:ElasticSearchManager
    {
        public bool AddSales(Admin admin, Sales sales, List<SalesInfo> salesInfo, bool stock)
        {
            if (sales == null || salesInfo == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                
                var elasticClient = GetElasticClient();

                var salesResponse = elasticClient.Index<Sales>(sales, i => i
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_SALES)
                );

                var insertDescriptor = new BulkDescriptor();

                foreach (var item in salesInfo)
                {
                    item.salesid = sales.salesid;
                    
                    insertDescriptor.Index<SalesInfo>(i => i
                    .Index(ElasticMappingConstants.INDEX_NAME)
                    .Type(ElasticMappingConstants.TYPE_SALES_INFO)
                    .Document(item)
                    );
                }


                var bulkResponse = elasticClient.Bulk(insertDescriptor);

                return salesResponse.RequestInformation.Success && bulkResponse.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool UpdateStock(List<SalesInfo> salesInfo) {
            try {

                var elasticClient = GetElasticClient();

                var updateDescriptor = new BulkDescriptor();

                foreach (var item in salesInfo) {
                    
                }

            } catch (Exception e) {

                throw e;
            }

            return false;
        }
    }
}