using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class CouponData : Data
    {
        public static bool ValidateCoupon(string couponId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // check if coupon entered is present in Db
                string sql = @"SELECT COUNT(*) FROM Coupon WHERE CouponCode = '" + couponId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int res = Convert.ToInt32(cmd.ExecuteScalar());
                if (res == 0)
                {
                    // if coupon entered is NOT present in Db
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static void UseCoupon(string couponCode)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //remove used coupon from db
                string sql = @"DELETE FROM Coupon 
                                WHERE CouponCode = '" + couponCode + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
