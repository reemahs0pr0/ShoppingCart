using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class WishData : Data
    {
        public static List<Wish> GetWishList(string userId)
        {
            //create list to store all records from Cart
            List<Wish> wishList = new List<Wish>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.ProductId, p.Image, p.Title, p.Description
                                FROM Product p, Wishlist w WHERE p.ProductId = w.ProductId 
                                 AND w.UserId = '" + userId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Wish wish = new Wish()
                    {
                        Id = (int)reader["ProductId"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                    };
                    wishList.Add(wish);
                }
            }            
            return wishList;
        }

        public static void AddItem(string userId, int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //create new record for user's wish list
                string sql = @"INSERT INTO WishList (UserId, ProductId)
                             VALUES (@UserId, @ProductId)";
                SqlCommand cmd2 = new SqlCommand(sql, conn);
                cmd2.Parameters.AddWithValue("@userId", userId);
                cmd2.Parameters.AddWithValue("@ProductId", productId);
                cmd2.ExecuteNonQuery();
            }
        }
        public static void RemoveItem(string userId, int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //delete record for the particular user and item
                string sql = @"DELETE FROM WishList WHERE ProductId = " + productId +
                                " AND UserID = '" + userId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
