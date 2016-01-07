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
    }
}