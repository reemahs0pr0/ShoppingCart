using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DAL;
using ShoppingCart.Db;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CouponController : Controller
    {
        private readonly DbGallery db;
        private readonly CouponsDAL couponsDAL;

        public CouponController(DbGallery db)
        {
            this.db = db;
            couponsDAL = new CouponsDAL(db);
        }

        [HttpPost]
        public IActionResult ValidateCoupon([FromBody] Coupon coupon)
        {
            // invoke action to validate coupon
            int couponLeft = couponsDAL.ValidateCoupon(coupon.Id);

            if (couponLeft != 0 && HttpContext.Session.GetString("couponcode") == null)
            {
                HttpContext.Session.SetString("couponcode", coupon.Id);
                couponsDAL.UseCoupon(coupon.Id);

                // if user entered coupon is validated
                return Json(new
                {
                    message = "You will receive 10% off on 'Checkout'"
                });
            }
            else if (HttpContext.Session.GetString("couponcode") != null)
            {
                // if user entered coupon again
                return Json(new
                {
                    message = "You have submitted a coupon!"
                });
            }
            else
            {
                // if user entered invalid coupon
                return Json(new
                {
                    message = "Invalid Coupon"
                });
            }
        }
    }
}
