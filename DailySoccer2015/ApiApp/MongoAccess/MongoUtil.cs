using MongoDB.Driver;
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
        private static IMongoClient _client;
        private static IMongoDatabase _database;

        static MongoUtil()
        {
            var connectionString = WebConfigurationManager.AppSettings["primaryConnectionString"];
            _client = new MongoClient(connectionString);

            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            _database = _client.GetDatabase(dbName);
        }

        public static string /*MongoCollection*/ GetCollection(string collectionName)
        {
            //var connectionString = WebConfigurationManager.ConnectionStrings["primaryConnectionString"].ConnectionString;
            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            return null;
        }
    }
}
