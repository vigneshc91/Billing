using BillingSoftware.Constants;
using BillingSoftware.Helper;
using BillingSoftware.Managers;
using BillingSoftware.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillingSoftware.Controllers
{
    public class SalesController : Controller
    {

        SalesManager salesManager = new SalesManager();

        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AddSales()
        {
            var billNo = Request.Params.Get(AppConstants.BILL_NO);
            var dateTime = Request.Params.Get(AppConstants.DATE_TIME);
            var salesInfo = Request.Params.Get(AppConstants.SALES_INFO);
            var tax = Request.Params.Get(AppConstants.TAX);
            var discount = Request.Params.Get(AppConstants.DISCOUNT);
            var maintainStock = Request.Params.Get(AppConstants.MAINTAIN_STOCK);

            float taxFloat, discountFloat = 0;
            DateTime date;
            bool maintainStockBool;
            var response = new ServiceResponse();
            var admin = CookieHelper.GetLoggedInAdmin(HttpContext);
            if(admin == null)
            {
                response.result = ErrorConstants.ADMIN_NOT_LOGGED_IN;
                return Json(response);
            }
            if(String.IsNullOrWhiteSpace(billNo) || String.IsNullOrWhiteSpace(dateTime) || String.IsNullOrWhiteSpace(salesInfo) || String.IsNullOrWhiteSpace(tax))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if(!DateTime.TryParse(dateTime, out date))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            }
            if(!float.TryParse(tax, out taxFloat))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            }
            if(!String.IsNullOrWhiteSpace(discount) && !float.TryParse(discount, out discountFloat))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            }
            maintainStockBool = bool.TryParse(maintainStock, out maintainStockBool) ? maintainStockBool : false;

            var sales = new List<SalesInfo>();

            try
            {
                var bill = new Sales() {
                    salesid = Guid.NewGuid(),
                    billno = billNo,
                    date = date,
                    tax = taxFloat,
                    discount = discountFloat,
                    status = (short) BillingEnums.STATUS.CLEARED,
                    created_at = DateTime.UtcNow
                    
                };
                sales = JsonConvert.DeserializeObject<List<SalesInfo>>(salesInfo);
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