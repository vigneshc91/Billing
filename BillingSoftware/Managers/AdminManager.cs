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
                throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

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
            if (String.IsNullOrWhiteSpace(userName)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.SUPER_ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

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

                var response = elasticClient.Index<Admin>(newAdmin, i => i
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

        public string GenerateAdminPassword(Admin admin, Guid userId)
        {
            if (userId == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.SUPER_ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var updateAdmin = GetAdminById(userId);
                if (updateAdmin == null) throw new Exception(ErrorConstants.ADMIN_NOT_FOUND);

                var password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                updateAdmin.salt = PasswordHash.GenerateSalt();
                updateAdmin.password = PasswordHash.CreateHash(password, updateAdmin.salt);

                var adminUpdate = new Dictionary<string, object>();
                adminUpdate[ConstAdmin.SALT] = updateAdmin.salt;
                adminUpdate[ConstAdmin.PASSWORD] = updateAdmin.password;

                var elasticClient = GetElasticClient();

                var updateResponse = elasticClient.Update<Admin, object>(u => u
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Id(updateAdmin.id.ToString())
                .Doc(adminUpdate));

                if (!updateResponse.RequestInformation.Success)
                    throw new Exception(ErrorConstants.PROBLEM_OCCURED_WHILE_GENERATING_PASSWORD);

                return password;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Admin GetAdminById(Guid userId)
        {
            if (userId == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                Admin admin = null;

                var elasticClient = GetElasticClient();

                var response = elasticClient.Get<Admin>(a => a
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Id(userId.ToString()));

                if (response.Found)
                {
                    admin = response.Source;
                }

                return admin;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool ChangePassword(Admin admin, Guid userId, string oldPassword, string newPassword)
        {
            if (userId == null || String.IsNullOrWhiteSpace(oldPassword) || String.IsNullOrWhiteSpace(newPassword)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null) throw new Exception(ErrorConstants.ADMIN_NOT_LOGGED_IN);

            try
            {
                var updateAdmin = GetAdminById(userId);
                if (updateAdmin == null)
                    throw new Exception(ErrorConstants.ADMIN_NOT_FOUND);

                if (!PasswordHash.ValidatePassword(oldPassword, updateAdmin.password, updateAdmin.salt))
                    throw new Exception(ErrorConstants.WRONG_PASSWORD);

                var newPasswordHash = PasswordHash.CreateHash(newPassword, updateAdmin.salt);

                var elasticClient = GetElasticClient();
                var passwordDict = new Dictionary<string, object>();
                passwordDict[ConstAdmin.PASSWORD] = newPasswordHash;

                var response = elasticClient.Update<Admin, object>(u => u
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Id(userId.ToString())
                .Doc(passwordDict));

                if (response.RequestInformation.Success)
                    return true;

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool DeleteAdmin(Admin admin, Guid adminId)
        {
            if (adminId == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.SUPER_ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var deleteAdmin = GetAdminById(adminId);
                if (deleteAdmin == null)
                    throw new Exception(ErrorConstants.ADMIN_NOT_FOUND);

                var elasticClient = GetElasticClient();
                var response = elasticClient.Delete<Admin>(adminId.ToString(), d => d
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN));

                if (response.RequestInformation.Success)
                    return true;

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Admin> GetAdminList(Admin admin)
        {
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.SUPER_ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Admin>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ADMIN)
                .Filter(f => f.Term(ConstAdmin.TYPE, (int)BillingEnums.USER_TYPE.ADMIN))
                .SortAscending(a => a.username)
                .Size(int.MaxValue));

                if (!response.RequestInformation.Success)
                    throw new Exception(ErrorConstants.PROBLEM_OCCURES_ON_RETRIVING_ADMIN_LIST);

                var adminList = new List<Admin>();

                foreach (var hit in response.Hits)
                {
                    var resultAdmin = hit.Source;
                    resultAdmin.salt = null;
                    resultAdmin.password = null;
                    adminList.Add(resultAdmin);
                }

                return adminList;
            }
            catch (Exception e )
            {

                throw e;
            }
            
        }
    }
}