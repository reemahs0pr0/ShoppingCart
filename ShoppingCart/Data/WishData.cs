using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class WishData : Data
    {
        public static List<Product> GetWishList(List<Product> productList, int userId) //generates a product list with proper Wish boolean
        {
            List<Product> prodWishList = productList;
            
            List<Wish> wishList = new List<Wish>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"Select ProductId FROM WishList Where userId = '" + userId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Wish wish = new Wish()
                    {
                        ProductId = (int)reader["ProductId"]
                    };
                    wishList.Add(wish);
                }
            }
            foreach (Product product in prodWishList)
            {
                foreach (Wish wish in wishList) //creating boolean for Wish parameter in the Product
                {
                    if(product.Id == wish.ProductId)
                    {
                        product.Wish = true; //if product is in wishlist, will be true
                    }
                }
            }
            return prodWishList;
            
        }
        
        public static void AddItem(string userId, int productId) // Adds item to wishlist
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
        public static void RemoveItem(string userId, int productId) // removes item from wishlist
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
