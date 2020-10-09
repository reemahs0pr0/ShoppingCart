using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class UserData : Data
    {
        public static string FindUser(ShoppingCart.Models.User userModel)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select count(*) from [User] where UserName ='" + userModel.Username
                    + "'AND Password ='" + userModel.Password + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                Int32 checkUser = (Int32)cmd.ExecuteScalar();

                if (checkUser == 0)
                {
                    return null;
                }
                else
                {
                    string sql2 = @"select UserID from [User] where UserName ='" + userModel.Username
                    + "'AND Password ='" + userModel.Password + "'";
                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    string userId = Convert.ToString(cmd2.ExecuteScalar());

                    return userId;
                }
            }
        }
    }
}
