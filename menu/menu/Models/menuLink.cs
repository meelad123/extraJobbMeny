using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace menu.Models
{
    public class menuLink
    {
        public int ID { get; set; }
        public string menuText { get; set; }
        public string menuURl { get; set; }
        public int? parentId { get; set; }
        public bool last { get; set; }

        /*Metod: Hämtar alla länkarna från databasen och sparar dem i en lista*/
        public static List<menuLink> getAllLinks(int roleId)
        {
            //string som har en sql query
            string SQL = "SELECT * FROM [dbo].[menuLinks] WHERE [roleId] = " + roleId + " ORDER BY [ID]";
            List<menuLink> results = new List<menuLink>();

            //Kopplar till databasen som har en connection string som heter menuLinks
            string _connectionString = DataSource.GetConnectionString("menuLinks");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(SQL, con); //exekverar sql queryn
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    menuLink menuLinksDB = new menuLink();
                    menuLinksDB.ID = Convert.ToInt32(dar["ID"] );
                    menuLinksDB.menuText = dar["Text"] as string;
                    menuLinksDB.menuURl = dar["Url"] as string;
                    if (!(dar["ParentId"] is DBNull))
                    {
                        menuLinksDB.parentId = Convert.ToInt32(dar["ParentId"]);
                    }
                    else 
                    {
                        menuLinksDB.parentId = 0;
                    }
                    menuLinksDB.last = Convert.ToBoolean(dar["Last"]);

                    results.Add(menuLinksDB);
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return results;
        }
    }
}