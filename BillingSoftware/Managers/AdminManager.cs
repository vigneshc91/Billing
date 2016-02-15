using BillingSoftware.Constants;
using BillingSoftware.Helper;
using BillingSoftware.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class AdminManager: ElasticSearchManager
    {
        public Admin LoginAdmin(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
                throw new Exception();

            try
            {
                var elasticClient = GetElasticClient();

                var userNameFilter = new TermFilter() {
                    Field = ConstAdmin.USER_NAME,
                    Value = userName
                };

                var passwordFilter = new TermFilter() {
                    Field = ConstAdmin.PASSWORD,
                    Value = password
                };

                var loginFilters = new List<FilterContainer>();
                loginFilters.Add(userNameFilter);
                loginFilters.Add(passwordFilter);

                var loginFilter = new AndFilter();
                loginFilter.Filters = loginFilters;

                var loginResponse = elasticClient.Search<Admin>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Filter(loginFilter)
                .Size(1));

                var admin = new Admin();

                if(loginResponse.Total > 0)
                {
                    foreach (IHit<Admin> hit in loginResponse.Hits)
                    {
                        admin = hit.Source;
                    }
                    return admin;
                }

                return admin;


            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
                return null;
            }
        }

       public bool LogoutAdmin(string token)
        {
            if (String.IsNullOrEmpty(token))
                return false;
            try
            {
                SessionHelper.RemoveSession<Admin>();
                return true;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                throw new Exception(ErrorConstants.PROBLEM_LOGOUT);
            }
        }
    }
}