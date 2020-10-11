using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class PurchasesData : Data
    {
        public static List<ActivationCode> GetActivationCodes(string userId)
        {
            //creates a list to store activation codes based on requested userId
            List<ActivationCode> codes = new List<ActivationCode>();

            string sql = @"SELECT ActivationCode, o.OrderId, ProductId 
                            FROM ActivationCode ac, [Order] o 
                              WHERE ac.OrderId = o.OrderId AND UserId = " + userId;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ActivationCode code = new ActivationCode()
                    {
                        Code = (string)reader["ActivationCode"],
                        OrderId = (int)reader["OrderId"],
                        ProductId = (int)reader["ProductId"]
                    };
                    codes.Add(code);
                }
            }
            return codes;
        }

        public static List<Purchases> GetPurchases(string userId) 
        {
            // create a list of purchases
            List<Purchases> purchases = new List<Purchases>();

            string sql = @"SELECT o.OrderId, od.ProductId, Quantity, o.UserId, PurchaseDate, [Image], Title, [Description], Link
                            FROM
                            (
	                            (SELECT OrderId, ProductId, Quantity FROM [Order Details]) od
	                            FULL OUTER JOIN
	                            (SELECT PurchaseDate, OrderId, UserId FROM [Order]) o ON od.OrderId = o.OrderId 
	                            FULL OUTER JOIN
	                            (SELECT [Image], Title, [Description], ProductId, Link FROM Product) p ON  od.ProductId = p.ProductId
                            ) 
                            WHERE o.UserId = " + userId + " ORDER BY o.OrderId DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Purchases purchase = new Purchases()
                    {
                        OrderId = (int)reader["OrderId"],
                        ProductId = (int)reader["ProductId"],
                        Quantity = (int)reader["Quantity"],
                        PurchaseDate = (string)reader["PurchaseDate"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Link = (string)reader["Link"],
                    };
                    purchases.Add(purchase);
                }
                conn.Close();
            }
            return purchases;
        }

        public static void AddOrder(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //create new record for user for the new order
                string sql = @"INSERT INTO [Order] (UserId, PurchaseDate) 
                                VALUES (@UserId, @PurchaseDate)";
                SqlCommand cmd2 = new SqlCommand(sql, conn);
                cmd2.Parameters.AddWithValue("@UserId", userId);
                cmd2.Parameters.AddWithValue("@PurchaseDate", DateTime.Now.ToString("dd-MM-yyyy"));
                cmd2.ExecuteNonQuery();
            }
        }
        public static void AddOrderDetails(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //obtain orderId from Order table
                string sql = @"SELECT MAX(OrderId) FROM [Order] where userId = " + userId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                int orderId = (int)cmd.ExecuteScalar();

                //copy records from Cart into Order Details
                string sql2 = @"INSERT INTO [Order Details] (OrderId, ProductId, Quantity)
                                 SELECT " + orderId + ", c.ProductId, c.Quantity " +
                                 "FROM [Order] o, Cart c WHERE c.UserId = o.UserId " +
                                  "AND OrderId = " + orderId;
                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                cmd2.ExecuteNonQuery();
            }
        }
        public static void AddActivationCode(ActivationCode code, string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //obtain orderId from Order table
                string sql = @"SELECT MAX(OrderId) FROM [Order] where userId = " + userId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                int orderId = (int)cmd.ExecuteScalar();

                //insert created activation codes of every item into record 
                string sql2 = @"INSERT INTO ActivationCode (ActivationCode, OrderId,  ProductId)
                                 VALUES (@ActivationCode, @OrderId, @ProductId)";
                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                cmd2.Parameters.AddWithValue("@ActivationCode", code.Code);
                cmd2.Parameters.AddWithValue("@OrderId", orderId);
                cmd2.Parameters.AddWithValue("@ProductId", code.ProductId);
                cmd2.ExecuteNonQuery();
            }
        }
    }
}
