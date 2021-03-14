using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Areas.Restaurant.Model;
using RestaurantManagementSystem.Areas.Restaurant.Data;

namespace RestaurantManagementSystem.Areas.Restaurant.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly RestaurantQuery _res;
        // GET: Restaurant/Restaurant

        public RestaurantController()
        {
            _res = new RestaurantQuery();
        }
        public ActionResult Booking()
        {
            return View();
        }

        public JsonResult CheckAvailability(AvailableCheck data)
        {
            List<LocationEntity> AvailableL = new List<LocationEntity>();
            AvailableL = _res.checkavailability(data);
            if(AvailableL!=null && AvailableL.Count()>0)
            {
                return Json((object)new { Status = true, data = AvailableL }, JsonRequestBehavior.AllowGet);
            }
            else return Json((object)new { Status = false, data = AvailableL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Booknow(BookingData model)
        {
            response response = _res.BookTable(model);
            if(response.Status)
            {
                TempData["BookingReference"] = response.response_id;
            }
            
            return Json((object)new { Status = response.Status, data = response }, JsonRequestBehavior.AllowGet);
        }

        
    }
}