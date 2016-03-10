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

                    
                    //insertDescriptor.Index<SalesInfo>(item, i => i
                    //.Index(ElasticMappingConstants.INDEX_NAME)
                    //.Type(ElasticMappingConstants.TYPE_SALES_INFO)
                    //);
                }


                var bulkResponse = elasticClient.Bulk(insertDescriptor);

                return bulkResponse.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}