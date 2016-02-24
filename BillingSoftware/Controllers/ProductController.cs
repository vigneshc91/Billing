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
            if(!Double.TryParse(price, out priceDouble) || !float.TryParse(quantity, out quantityFloat) || Int16.TryParse(unit, out unitInt))
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
                respone.result = productManager.GetProductById(admin, id);
                respone.status = true;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }

            return Json(respone);
        }
    }
}