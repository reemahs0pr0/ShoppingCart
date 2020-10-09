using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class UserData : Data
    {
        public static string FindUserId(ShoppingCart.Models.User userModel)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //check if there is such user
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
                    //if found user, return its userId
                    string sql2 = @"select UserId from [User] where UserName ='" + userModel.Username
                    + "'AND Password ='" + userModel.Password + "'";
                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    string userId = Convert.ToString(cmd2.ExecuteScalar());

                    return userId;
                }
            }
        }
        public static string FindName(string userId)
        {
            //connect to database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //extract name from database
                string sql2 = @"select Name from [User] where Userid = '" + userId + "'";
                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                string name = Convert.ToString(cmd2.ExecuteScalar());

                return name;

            }
        }
    }
}
