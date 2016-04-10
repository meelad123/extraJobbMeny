using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace menu
{
    public class DataSource
    {
        public static string GetConnectionString(string name)
        {
            //Requires references to System.Web and System.Configuration  
            return System.Web.Configuration.WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}