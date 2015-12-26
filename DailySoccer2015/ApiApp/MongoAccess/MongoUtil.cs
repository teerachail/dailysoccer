using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ApiApp.MongoAccess
{
    static class MongoUtil
    {
        public static string /*MongoCollection*/ GetCollection(string collectionName)
        {
            //var connectionString = WebConfigurationManager.ConnectionStrings["primaryConnectionString"].ConnectionString;
            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            return null;
        }
    }
}
