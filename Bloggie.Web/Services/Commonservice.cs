using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bloggie.Web.Services
{
    public class Commonservice
    {
        public static string getConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string connString = configuration.GetConnectionString("dbConnection");
            return connString;
        }
    }
}