using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Db;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CouponController : Controller
    {
        private readonly DbGallery db;

        public CouponController(DbGallery db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult ValidateCoupon([FromBody] Coupon coupon)
        {
            // invoke action to validate coupon
            int couponLeft = db.Coupons.Where(x => x.Id == coupon.Id).Count();

            if (couponLeft != 0 && HttpContext.Session.GetString("couponcode") == null)
            {
                HttpContext.Session.SetString("couponcode", coupon.Id);
                Coupon usedCoupon = (Coupon)db.Coupons.Where(x => x.Id == coupon.Id);
                db.Coupons.Remove(usedCoupon);
                db.SaveChanges();

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
