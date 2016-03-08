using BillingSoftware.Constants;
using BillingSoftware.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class UserManager:ElasticSearchManager
    {
        public bool AddUser(Admin admin, User user)
        {
            if (user == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var userExist = GetUserById(admin, user.userid);
                if (userExist != null) throw new Exception(ErrorConstants.USER_WITH_GIVEN_ID_ALREADY_EXIST);

                var response = elasticClient.Index<User>(user, i => i
                 .Index(ElasticMappingConstants.INDEX_NAME)
                 .Type(ElasticMappingConstants.TYPE_USER)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public User GetUserById(Admin admin, string userid)
        {
            if (String.IsNullOrWhiteSpace(userid)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Filter(f => f.Term(ConstUser.ID, userid))
                .Size(1));

                User retUser = null;

                if(response.Total > 0)
                    foreach (var item in response.Hits)
                        retUser = item.Source;

                return retUser;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public string GetUserId(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Filter(f => f.Term(ConstUser.ID, id))
                .Size(1));

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        return item.Id;

                return null;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool  UpdateUser(Admin admin, User user)
        {
            if (user == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var userElasticId = GetUserId(user.userid);
                if (String.IsNullOrWhiteSpace(userElasticId)) throw new Exception(ErrorConstants.USER_NOT_FOUND);

                var userUpdate = new Dictionary<string, object>();

                if (!String.IsNullOrWhiteSpace(user.name))
                    userUpdate[ConstUser.NAME] = user.name;
                if (!String.IsNullOrWhiteSpace(user.addr1))
                    userUpdate[ConstUser.ADDR_1] = user.addr1;
                if (!String.IsNullOrWhiteSpace(user.addr2))
                    userUpdate[ConstUser.ADDR_2] = user.addr2;
                if (!String.IsNullOrWhiteSpace(user.city))
                    userUpdate[ConstUser.CITY] = user.city;
                if (!String.IsNullOrWhiteSpace(user.district))
                    userUpdate[ConstUser.DISTRICT] = user.district;
                if (!String.IsNullOrWhiteSpace(user.state))
                    userUpdate[ConstUser.STATE] = user.state;
                if (!String.IsNullOrWhiteSpace(user.country))
                    userUpdate[ConstUser.COUNTRY] = user.country;
                if (!String.IsNullOrWhiteSpace(user.pincode))
                    userUpdate[ConstUser.PIN_CODE] = user.pincode;
                if (!String.IsNullOrWhiteSpace(user.phone))
                    userUpdate[ConstUser.PHONE] = user.phone;
                if (!String.IsNullOrWhiteSpace(user.email))
                    userUpdate[ConstUser.EMAIL] = user.email;

                var elasticClient = GetElasticClient();

                var response = elasticClient.Update<User, object>(u => u
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Id(userElasticId)
                .Doc(userUpdate)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool DeleteUser(Admin admin, String id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var userElasticId = GetUserId(id);
                if (String.IsNullOrWhiteSpace(userElasticId)) throw new Exception(ErrorConstants.USER_NOT_FOUND);

                var elasticClient = GetElasticClient();

                var response = elasticClient.Delete<User>(userElasticId, d => d
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<User> GetUserList(Admin admin, int start, int size)
        {
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Skip(start)
                .Take(size)
                );

                var userList = new List<User>();
                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        userList.Add(item.Source);

                return userList;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<User> GetUsersByName(Admin admin, string name, int start, int size)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Query(q => q.Prefix(ConstUser.NAME, name))
                .Skip(start)
                .Take(size)
                );

                var userList = new List<User>();

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        userList.Add(item.Source);

                return userList;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}