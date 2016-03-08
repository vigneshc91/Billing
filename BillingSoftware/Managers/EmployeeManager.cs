using BillingSoftware.Constants;
using BillingSoftware.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class EmployeeManager:ElasticSearchManager
    {
        public bool AddEmployee(Admin admin, Employee employee)
        {
            if (employee == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var employeeExist = GetEmployeeById(admin, employee.employeeid);
                if (employeeExist != null) throw new Exception(ErrorConstants.EMPLOYEE_WITH_GIVEN_ID_ALREADY_EXIST);

                var response = elasticClient.Index<Employee>(employee, i => i
                 .Index(ElasticMappingConstants.INDEX_NAME)
                 .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Employee GetEmployeeById(Admin admin, string employeeid)
        {
            if (String.IsNullOrWhiteSpace(employeeid)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Employee>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                .Filter(f => f.Term(ConstEmployee.ID, employeeid))
                .Size(1));

                Employee retEmployee = null;

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        retEmployee = item.Source;

                return retEmployee;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public string GetEmployeeId(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Employee>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                .Filter(f => f.Term(ConstEmployee.ID, id))
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

        public bool UpdateEmployee(Admin admin, Employee employee)
        {
            if (employee == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var employeeElasticId = GetEmployeeId(employee.employeeid);
                if (String.IsNullOrWhiteSpace(employeeElasticId)) throw new Exception(ErrorConstants.EMPLOYEE_NOT_FOUND);

                var employeeUpdate = new Dictionary<string, object>();

                if (!String.IsNullOrWhiteSpace(employee.name))
                    employeeUpdate[ConstEmployee.NAME] = employee.name;
                if (!String.IsNullOrWhiteSpace(employee.addr1))
                    employeeUpdate[ConstEmployee.ADDR_1] = employee.addr1;
                if (!String.IsNullOrWhiteSpace(employee.addr2))
                    employeeUpdate[ConstEmployee.ADDR_2] = employee.addr2;
                if (!String.IsNullOrWhiteSpace(employee.city))
                    employeeUpdate[ConstEmployee.CITY] = employee.city;
                if (!String.IsNullOrWhiteSpace(employee.district))
                    employeeUpdate[ConstEmployee.DISTRICT] = employee.district;
                if (!String.IsNullOrWhiteSpace(employee.state))
                    employeeUpdate[ConstEmployee.STATE] = employee.state;
                if (!String.IsNullOrWhiteSpace(employee.country))
                    employeeUpdate[ConstEmployee.COUNTRY] = employee.country;
                if (!String.IsNullOrWhiteSpace(employee.pincode))
                    employeeUpdate[ConstEmployee.PIN_CODE] = employee.pincode;
                if (!String.IsNullOrWhiteSpace(employee.phone))
                    employeeUpdate[ConstEmployee.PHONE] = employee.phone;
                if (!String.IsNullOrWhiteSpace(employee.email))
                    employeeUpdate[ConstEmployee.EMAIL] = employee.email;
                if (!String.IsNullOrWhiteSpace(employee.designation))
                    employeeUpdate[ConstEmployee.DESIGNATION] = employee.designation;
                var elasticClient = GetElasticClient();

                var response = elasticClient.Update<Employee, object>(u => u
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                .Id(employeeElasticId)
                .Doc(employeeUpdate)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool DeleteEmployee(Admin admin, String id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var employeeElasticId = GetEmployeeId(id);
                if (String.IsNullOrWhiteSpace(employeeElasticId)) throw new Exception(ErrorConstants.EMPLOYEE_NOT_FOUND);

                var elasticClient = GetElasticClient();

                var response = elasticClient.Delete<Employee>(employeeElasticId, d => d
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Employee> GetEmployeeList(Admin admin, int start, int size)
        {
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Employee>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                .Skip(start)
                .Take(size)
                );

                var employeeList = new List<Employee>();
                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        employeeList.Add(item.Source);

                return employeeList;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Employee> GetEmployeesByName(Admin admin, string name, int start, int size)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            if (admin == null || admin.type != (int)BillingEnums.USER_TYPE.ADMIN) throw new Exception(ErrorConstants.NO_PREVILAGE);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Employee>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_EMPLOYEE)
                .Query(q => q.Prefix(ConstEmployee.NAME, name))
                .Skip(start)
                .Take(size)
                );

                var employeeList = new List<Employee>();

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        employeeList.Add(item.Source);

                return employeeList;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}