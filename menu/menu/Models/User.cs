using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace menu.Models
{
    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public int role { get; set; }

        //skapar en användare i databasen
        public static void createUser(User u)
        {
            string SQL = "INSERT INTO [dbo].[menuUser]([userId],[username],[userPass],[roleId]) " +
                         "VALUES" + " (" + u.userId + ", '" + u.userName + "', '" + u.password + "', '" + u.role + ");";
            string _connectionString = DataSource.GetConnectionString("menuLinks");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
        }


        //Kollar om en användare finns i databasen, om den finns så hämtas den och sparas i en user-objekt
        public User checkUser(User u)
        {
            User toreturn;
            string SQL = "SELECT * FROM [dbo].[users-table] where username='" + u.userName + "';";
            string _connectionString = DataSource.GetConnectionString("menuLinks");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.HasRows)
                {
                    User newUser = new User();
                    while (dar.Read())
                    {
                        newUser.userName = dar["username"].ToString();
                        newUser.password = dar["userPass"].ToString();
                        newUser.role = Convert.ToInt32(dar["roleId"]);
                        newUser.userId = Convert.ToInt32(dar["userId"]);
                    }
                    toreturn = newUser;
                }
                else
                    toreturn = null;
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return toreturn;
        }
    }

}