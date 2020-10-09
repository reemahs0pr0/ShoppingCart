using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class CartData : Data
    {
        public static List<Cart> GetCart(string userId)
        {
            //create list to store all records from Cart
            List<Cart> cart = new List<Cart>();

            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.ProductId, p.Image, p.Title, p.Description, p.Price, c.Quantity
                                FROM Product p, Cart c WHERE p.ProductId = c.ProductId 
                                 AND c.UserId = '" + userId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //store each record as Cart object
                    Cart item = new Cart()
                    {
                        Id = (int)reader["ProductId"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"]
                    };
                    cart.Add(item);
                }
            }
            //return list to controller
            return cart;
        }
        public static void AddItem(string userId, int productId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //check if record exist
                string check = @"SELECT COUNT(*) FROM Cart WHERE ProductId = " + productId + 
                                    " AND UserId = '" + userId + "'";
                SqlCommand cmd1 = new SqlCommand(check, conn);
                int checkRow = Convert.ToInt32(cmd1.ExecuteScalar());

                if (checkRow == 0)
                {
                    //create new record for user for the new item
                    string sql = @"INSERT INTO Cart (UserId, ProductId, Quantity) 
                                    VALUES (@UserId, @ProductId, @Quantity)";
                    SqlCommand cmd2 = new SqlCommand(sql, conn);
                    cmd2.Parameters.AddWithValue("@UserId", userId);
                    cmd2.Parameters.AddWithValue("@ProductId", productId);
                    cmd2.Parameters.AddWithValue("@Quantity", 1);
                    cmd2.ExecuteNonQuery();
                }
                else
                {
                    //+1 quantity if item already exist in the cart for the user
                    string sql = @"UPDATE Cart SET Quantity = Quantity + 1 WHERE ProductId = " + 
                                    productId + " AND UserId = '" + userId + "'";
                    SqlCommand cmd2 = new SqlCommand(sql, conn);
                    cmd2.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateQuantity(string userId, int productId, int quantity)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //update quantity for the particular user and item
                string sql = @"UPDATE Cart SET Quantity = " + quantity + 
                                " WHERE ProductId = " + productId + " AND UserId = '" + userId + 
                                    "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        public static void RemoveItem(string userId, int productId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //delete record for the particular user and item
                string sql = @"DELETE FROM Cart WHERE ProductId = " + productId +
                                " AND UserID = '" + userId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        public static int CheckLastInCart(string userId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //check if user has items in cart from previous visit
                string check = @"SELECT COUNT(*) FROM Cart WHERE UserId = '" + userId + "'";
                SqlCommand cmd1 = new SqlCommand(check, conn);
                int checkRow = Convert.ToInt32(cmd1.ExecuteScalar());

                if (checkRow == 0)
                {
                    return 0;
                }
                else
                {
                    //if there are, sum the quantity for cart icon display
                    string sql = @"SELECT SUM(Quantity) FROM Cart WHERE UserId = '" + userId + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    int checkQuantity = Convert.ToInt32(cmd.ExecuteScalar());
                    return checkQuantity;
                }
            }
        }
        public static void DeleteCart(string userId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //delete all records for the particular user
                string sql = @"DELETE FROM Cart WHERE UserID = '" + userId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        public static void UpdateId(string userId, string guid)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //update userId for record based on GUID used as guest
                string sql = @"UPDATE Cart SET UserId = '" + userId + "' WHERE UserId = '" +
                                guid + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}