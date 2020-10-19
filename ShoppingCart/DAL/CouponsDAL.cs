using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class CouponsDAL
    {
        private readonly DbGallery db;

        public CouponsDAL(DbGallery db)
        {
            this.db = db;
        }

        public int ValidateCoupon(string couponId)
        {
            int couponLeft = db.Coupons.Where(x => x.Id == couponId).Count();

            return couponLeft;
        }

        public void UseCoupon(string couponId)
        {
            Coupon usedCoupon = db.Coupons.Where(x => x.Id == couponId).Single();
            db.Coupons.Remove(usedCoupon);
            db.SaveChanges();
        }
    }
}
