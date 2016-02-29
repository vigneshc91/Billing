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
    public class ProductController : Controller
    {

        ProductManager productManager = new ProductManager();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AddProduct()
        {
            var id = Request.Params.Get(AppConstants.PRODUCT_ID);
            var name = Request.Params.Get(AppConstants.PRODUCT_NAME);
            var price = Request.Params.Get(AppConstants.PRICE);
            var quantity = Request.Params.Get(AppConstants.QUANTITY);
            var unit = Request.Params.Get(AppConstants.UNIT);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if(String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(price) || String.IsNullOrWhiteSpace(quantity) || String.IsNullOrWhiteSpace(unit))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            double priceDouble;
            float quantityFloat;
            Int16 unitInt;
            if(!Double.TryParse(price, out priceDouble) || !float.TryParse(quantity, out quantityFloat) || !Int16.TryParse(unit, out unitInt))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            } 

            try
            {
                var product = new Product()
                {
                    productid = id,
                    productname = name,
                    price = priceDouble,
                    quantity = quantityFloat,
                    unit = unitInt,
                    create_at = DateTime.UtcNow
                };

                if(productManager.AddProduct(admin, product))
                {
                    response.result = SuccessConstants.PRODUCT_ADDED;
                    response.status = true;
                } else
                    response.result = ErrorConstants.PROBLEM_ADDING_PRODUCT;

                return Json(response);

            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);

        }

        public JsonResult GetProductById()
        {
            var id = Request.Params.Get(AppConstants.PRODUCT_ID);

            var respone = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);

            if (String.IsNullOrWhiteSpace(id))
            {
                respone.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(respone);
            }
            if(admin == null)
            {
                respone.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(respone);
            }

            try
            {
                var product = productManager.GetProductById(admin, id);
                if(product != null)
                {
                    respone.result = product;
                    respone.status = true;
                } else
                respone.result = ErrorConstants.PRODUCT_NOT_FOUND;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                respone.result = e.GetBaseException().Message;
            }

            return Json(respone);
        }

        public JsonResult UpdateProduct()
        {
            var id = Request.Params.Get(AppConstants.PRODUCT_ID);
            var name = Request.Params.Get(AppConstants.PRODUCT_NAME);
            var price = Request.Params.Get(AppConstants.PRICE);
            var quantity = Request.Params.Get(AppConstants.QUANTITY);
            var unit = Request.Params.Get(AppConstants.UNIT);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if(String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(price) && String.IsNullOrWhiteSpace(quantity) && String.IsNullOrWhiteSpace(unit))
            {
                response.result = ErrorConstants.NO_CHANGES;
                return Json(response);
            }
            double priceDouble;
            float quantityFloat;
            Int16 unitInt;
            if(!double.TryParse(price, out priceDouble) || !float.TryParse(quantity, out quantityFloat) || !Int16.TryParse(unit, out unitInt))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            }
            try
            {
                var product = new Product();
                product.productid = id;

                if (!String.IsNullOrWhiteSpace(name))
                    product.productname = name;

                if (!String.IsNullOrWhiteSpace(price))
                    product.price = priceDouble;

                if (!String.IsNullOrWhiteSpace(quantity))
                    product.quantity = quantityFloat;

                if (!String.IsNullOrWhiteSpace(unit))
                    product.unit = unitInt;

                if(productManager.UpdateProduct(admin, product))
                {
                    response.result = SuccessConstants.PRODUCT_UPDATED;
                    response.status = true;
                } else
                response.result = ErrorConstants.PROBLEM_UPDATING_PRODUCT;

                return Json(response);

            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult DeleteProduct()
        {
            var id = Request.Params.Get(AppConstants.PRODUCT_ID);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);

            if(admin == null)
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
                if (productManager.DeleteProduct(admin, id))
                {
                    response.result = SuccessConstants.PRODUCT_DELETED;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.PROBLEM_DELETING_PRODUCT;

                return Json(response);

            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult GetProductList()
        {
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            int intStart, intSize;

            var response = new ServiceResponse();

            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);

            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }

            intStart = !String.IsNullOrWhiteSpace(start) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !String.IsNullOrWhiteSpace(size) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                response.result = productManager.GetProductList(admin, intStart, intSize);
                response.status = true;

                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult GetProductsByName()
        {
            var name = Request.Params.Get(AppConstants.PRODUCT_NAME);
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            int intStart, intSize;

            intStart = !String.IsNullOrWhiteSpace(start) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !String.IsNullOrWhiteSpace(size) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                response.result = productManager.GetProductByName(admin, name, intStart, intSize);
                response.status = true;

                return Json(response);
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }
    }
}