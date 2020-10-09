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
        public static List<ActivationCode> GetActivationCodes(string userId) // creates a list of activation codes based on requested userId
        {
            List<ActivationCode> codes = new List<ActivationCode>();

            string sql = @"select * from ActivationCode Where UserId = " + userId;

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
        public static List<Purchases> GetPurchases(string userId) // create a list of purchases
        {
            string sql = @"select OD.OrderId, OD.ProductId, Quantity, OD.UserId, PurchaseDate, [Image], Title, [Description], Link 
                            FROM
                            (
                             (Select OD.OrderId, OD.ProductId, OD.Quantity, OD.UserId from OrderDetails OD) OD
                             full outer join
                             (select PurchaseDate, O.orderId from [Order] O) O on OD.OrderId = O.OrderId 
                             full outer join
                             (select [image], title, [description], Link, P.ProductId From Product P) P on  OD.ProductId = P.ProductId
                            ) where OD.UserId = " + userId;


            List<Purchases> purchases = new List<Purchases>();

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
                        PurchaseDate = (DateTime)reader["PurchaseDate"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                    };
                    purchases.Add(purchase);
                }
                conn.Close();
            }

            return purchases;
        }
    }
}
