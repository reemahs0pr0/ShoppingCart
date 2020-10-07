using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartData : Data
    {
        public static List<Cart> GetCart(int userId)
        {
            //create list to store all records from Cart
            List<Cart> cart = new List<Cart>();

            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.ProductId, p.Image, p.Title, p.Description, p.Price, c.Quantity
                                FROM Product p, CartCopy c WHERE p.ProductId = c.ProductId 
                                 AND c.UserId = " + userId;
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
        public static void AddItem(int userId, int productId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //check if record exist
                string check = @"SELECT COUNT(*) FROM CartCopy WHERE ProductId = " + productId + 
                                    " AND UserId = " + userId;
                SqlCommand cmd1 = new SqlCommand(check, conn);
                int checkRow = Convert.ToInt32(cmd1.ExecuteScalar());

                if (checkRow == 0)
                {
                    //create new record for user for the new item
                    string sql = @"INSERT INTO CartCopy (UserId, ProductId, Quantity) 
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
                    string sql = @"UPDATE CartCopy SET Quantity = Quantity + 1 WHERE ProductId = " + 
                                    productId + " AND UserId = " + userId;
                    SqlCommand cmd2 = new SqlCommand(sql, conn);
                    cmd2.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateQuantity(int userId, int productId, int quantity)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //update quantity for the particular user and item
                string sql = @"UPDATE CartCopy SET Quantity = " + quantity + 
                                " WHERE ProductId = " + productId + " AND UserId = " + userId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        public static void RemoveItem(int userId, int productId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //delete record for the particular user and item
                string sql = @"DELETE FROM CartCopy WHERE ProductId = " + productId +
                                " AND UserID = " + userId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}