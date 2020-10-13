using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CouponController : Controller
    {
        [HttpPost]
        public IActionResult ValidateCoupon([FromBody] Coupon coupon)
        {
            string couponCode = coupon.CouponCode;
            // invoke action to validate coupon
            if (CouponData.ValidateCoupon(couponCode) == true && HttpContext.Session.GetString("couponcode") == null)
            {
                HttpContext.Session.SetString("couponcode", couponCode);
                CouponData.UseCoupon(couponCode);
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
