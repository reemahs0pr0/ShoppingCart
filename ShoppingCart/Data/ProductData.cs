using Microsoft.AspNetCore.Http;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class ProductData : Data
    {
        public static List<Product> GetAllProducts()
        {
            //create list to store all records from Product
            List<Product> productslist = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT ProductId, Image, Title, Description, Price 
                                FROM Product";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"]
                    };
                    productslist.Add(product);
                }

            }
            return productslist;
        }



        public static List<Product> GetProductSearch(string search)
        {
            //create list to store all records from Product based on search results
            List<Product> productslist = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT ProductId, Image, Title, Description, Price 
                               FROM Product
                                WHERE Description like'%" + search + "%'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"]
                    };
                    productslist.Add(product);
                }
            }
            return productslist;
        }
        public static List<Product> GetTopSellingProduct()
        {
            List<Product> topsellingproductlist = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT ProductId
                               FROM [Order Details]
                                GROUP BY ProductId
                                 ORDER BY sum(Quantity) DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = (int)reader["ProductId"]

                    };
                    topsellingproductlist.Add(product);
                }
            }
            return topsellingproductlist;
        }
    }
}
