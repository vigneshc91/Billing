using BillingSoftware.Constants;
using BillingSoftware.Helper;
using BillingSoftware.Models;
using Elasticsearch.Net.ConnectionPool;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class ElasticSearchManager
    {
        static Uri node = new Uri(ElasticMappingConstants.ELASTIC_SEARCH_URL);
        static bool isDbInitialized = false;
        static SniffingConnectionPool connectionPool = new SniffingConnectionPool(new[] { node });

        public ElasticSearchManager()
        {
            if (!isDbInitialized)
            {
                InitializeDb();
                isDbInitialized = true;
            }
        }

        private void InitializeDb()
        {
            var mappings = new ElasticSearchMappings();
            var elasticClient = GetElasticClient();
            try
            {
                if (!elasticClient.IndexExists(ElasticMappingConstants.INDEX_NAME).Exists)
                {
                    var indexCreationResponse = elasticClient.CreateIndex(i => i
                                                    .Index(ElasticMappingConstants.INDEX_NAME));

                    if(indexCreationResponse.ConnectionStatus.HttpStatusCode != 200)
                    {
                        throw new Exception(ErrorConstants.INDEX_CREATE_FAILED);
                    }

                    mappings.CheckMappings(elasticClient);
                    AddSuperAdmin();
                }
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }
        }

        public void AddSuperAdmin()
        {
            var elasticClient = GetElasticClient();

            var termFilter = new TermFilter()
            {
                Field = ConstAdmin.TYPE,
                Value = (short)BillingEnums.USER_TYPE.SUPER_ADMIN
            };

            try
            {
                var response = elasticClient.Search<Admin>(a => a
                                .Index(ElasticMappingConstants.INDEX_NAME)
                                .Type(ElasticMappingConstants.TYPE_ADMIN)
                                .Filter(termFilter)
                                .Take(1));

                if (response.Total == 0)
                {
                    var salt = PasswordHash.GenerateSalt();
                    var admin = new Admin()
                    {
                        id = Guid.NewGuid(),
                        username = AppConstants.SUPER_ADMIN_USER_NAME,
                        salt = salt,
                        password = PasswordHash.CreateHash(AppConstants.SUPER_ADMIN_PASSWORD, salt),
                        created_at = DateTime.UtcNow
                    };

                    var create = elasticClient.Index<Admin>(admin, i => i
                                    .Index(ElasticMappingConstants.INDEX_NAME)
                                    .Type(ElasticMappingConstants.TYPE_ADMIN));
                    
                }
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            
        }

        public ElasticClient GetElasticClient()
        {
            ConnectionSettings settings = new ConnectionSettings(connectionPool, ElasticMappingConstants.INDEX_NAME);
            settings.ThrowOnElasticsearchServerExceptions(true);
            return new ElasticClient(settings);
        }
    }
}