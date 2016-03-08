using BillingSoftware.Constants;
using BillingSoftware.Helper;
using BillingSoftware.Managers;
using BillingSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillingSoftware.Controllers
{
    public class EmployeeController : Controller
    {

        EmployeeManager employeeManager = new EmployeeManager();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AddEmployee()
        {
            var id = Request.Params.Get(AppConstants.EMPLOYEE_ID);
            var name = Request.Params.Get(AppConstants.NAME);
            var addr1 = Request.Params.Get(AppConstants.ADDRESS_1);
            var addr2 = Request.Params.Get(AppConstants.ADDRESS_2);
            var city = Request.Params.Get(AppConstants.CITY);
            var district = Request.Params.Get(AppConstants.DISTRICT);
            var state = Request.Params.Get(AppConstants.START);
            var country = Request.Params.Get(AppConstants.COUNTRY);
            var phone = Request.Params.Get(AppConstants.PHONE);
            var pinCode = Request.Params.Get(AppConstants.PIN_CODE);
            var email = Request.Params.Get(AppConstants.EMAIL);
            var designation = Request.Params.Get(AppConstants.DESIGNATION);

            var response = new ServiceResponse();

            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(name))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            try
            {
                var employee = new Employee()
                {
                    employeeid = id,
                    name = name,
                    addr1 = addr1,
                    addr2 = addr2,
                    city = city,
                    district = district,
                    state = state,
                    country = country,
                    pincode = pinCode,
                    phone = phone,
                    email = email,
                    designation = designation,
                    created_at = DateTime.UtcNow
                };

                if (employeeManager.AddEmployee(admin, employee))
                {
                    response.result = SuccessConstants.EMPLOYEE_ADDED;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.PROBLEM_ADDING_EMPLOYEE;

                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);

        }

        public JsonResult UpdateEmployee()
        {
            var id = Request.Params.Get(AppConstants.EMPLOYEE_ID);
            var name = Request.Params.Get(AppConstants.NAME);
            var addr1 = Request.Params.Get(AppConstants.ADDRESS_1);
            var addr2 = Request.Params.Get(AppConstants.ADDRESS_2);
            var city = Request.Params.Get(AppConstants.CITY);
            var district = Request.Params.Get(AppConstants.DISTRICT);
            var state = Request.Params.Get(AppConstants.START);
            var country = Request.Params.Get(AppConstants.COUNTRY);
            var phone = Request.Params.Get(AppConstants.PHONE);
            var pinCode = Request.Params.Get(AppConstants.PIN_CODE);
            var email = Request.Params.Get(AppConstants.EMAIL);
            var designation = Request.Params.Get(AppConstants.DESIGNATION);

            var response = new ServiceResponse();

            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(addr1) && String.IsNullOrWhiteSpace(addr2) && String.IsNullOrWhiteSpace(city) && String.IsNullOrWhiteSpace(district) && String.IsNullOrWhiteSpace(state) && String.IsNullOrWhiteSpace(country) && String.IsNullOrWhiteSpace(pinCode) && String.IsNullOrWhiteSpace(phone) && String.IsNullOrWhiteSpace(email) && String.IsNullOrWhiteSpace(designation))
            {
                response.result = ErrorConstants.NO_CHANGES;
                return Json(response);
            }

            try
            {
                var employee = new Employee()
                {
                    employeeid = id,
                    name = name,
                    addr1 = addr1,
                    addr2 = addr2,
                    city = city,
                    district = district,
                    state = state,
                    country = country,
                    pincode = pinCode,
                    phone = phone,
                    email = email,
                    designation = designation
                };

                if (employeeManager.UpdateEmployee(admin, employee))
                {
                    response.result = SuccessConstants.EMPLOYEE_UPDATED;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.PROBLEM_UPDATING_EMPLOYEE;

                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult DeleteEmployee()
        {
            var id = Request.Params.Get(AppConstants.EMPLOYEE_ID);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            try
            {
                if (employeeManager.DeleteEmployee(admin, id))
                {
                    response.result = SuccessConstants.EMPLOYEE_DELETED;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.PROBLEM_DELETING_EMPLOYEE;

                return Json(response);

            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult GetEmployeeById()
        {
            var id = Request.Params.Get(AppConstants.EMPLOYEE_ID);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            try
            {
                var employee = employeeManager.GetEmployeeById(admin, id);
                if (employee != null)
                {
                    response.result = employee;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.EMPLOYEE_NOT_FOUND;

                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult GetEmployeeList()
        {
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            int intStart, intSize;

            var respone = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                respone.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(respone);
            }

            intStart = !(String.IsNullOrWhiteSpace(start)) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !(String.IsNullOrWhiteSpace(size)) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                respone.result = employeeManager.GetEmployeeList(admin, intStart, intSize);
                respone.status = true;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                respone.result = e.GetBaseException().Message;
            }

            return Json(respone);

        }

        public JsonResult GetEmployeesByName()
        {
            var name = Request.Params.Get(AppConstants.NAME);
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            int intStart, intSize;

            var respone = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if (admin == null)
            {
                respone.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(respone);
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                respone.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(respone);
            }

            intStart = !(String.IsNullOrWhiteSpace(start)) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !(String.IsNullOrWhiteSpace(size)) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                respone.result = employeeManager.GetEmployeesByName(admin, name, intStart, intSize);
                respone.status = true;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                respone.result = e.GetBaseException().Message;
            }

            return Json(respone);
        }
    }
}