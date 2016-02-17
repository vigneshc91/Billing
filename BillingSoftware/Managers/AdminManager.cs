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

                var loginFilters = new List<FilterContainer>();
                loginFilters.Add(userNameFilter);

                var loginFilter = new AndFilter();
                loginFilter.Filters = loginFilters;

                var loginResponse = elasticClient.Search<Admin>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Filter(loginFilter)
                .Size(1));

                var admin = new Admin();

                if (loginResponse.Total > 0)
                {
                    foreach (IHit<Admin> hit in loginResponse.Hits)
                    {
                        admin = hit.Source;
                    }
                    if (!PasswordHash.ValidatePassword(password, admin.password, admin.salt))
                        throw new Exception(ErrorConstants.WRONG_PASSWORD);
                    return admin;
                }
                else
                    return null;


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

        public bool CreateAdmin(Admin admin, string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new Exception();
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.SUPER_ADMIN) throw new Exception();

            var newAdmin = new Admin()
            {
                id = Guid.NewGuid(),
                username = userName,
                type = (int)BillingEnums.USER_TYPE.ADMIN,
                created_at = DateTime.UtcNow
            };

            try
            {
                var elasticClient = GetElasticClient();

                var userNameFilter = new TermFilter()
                {
                    Field = ConstAdmin.USER_NAME,
                    Value = userName
                };

                var adminList = elasticClient.Search<Admin>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Filter(userNameFilter)
                );

                if(adminList.Total > 0)
                {
                    throw new Exception(ErrorConstants.ADMIN_USERNAME_ALREADY_TAKEN);
                }

                var response = elasticClient.Index<Admin>(admin, i => i
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                );

                return response.Created;

            }
            catch (Exception e)
            {
                
                Console.Error.WriteLine(e.GetBaseException().Message);
                throw e;
            }
        }
    }
}