using ApiApp.Models;
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
        #region Fields

        private static IMongoClient _client;
        private static IMongoDatabase _database;

        #endregion Fields

        #region Constructors

        static MongoUtil()
        {
            var connectionString = WebConfigurationManager.AppSettings["primaryConnectionString"];
            _client = new MongoClient(connectionString);

            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            _database = _client.GetDatabase(dbName);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ดึงข้อมูลจากตาราง
        /// </summary>
        /// <typeparam name="T">ข้อมูลที่ทำงานด้วย</typeparam>
        /// <param name="tableName">ชื่อตาราง</param>
        public static IMongoCollection<T> GetCollection<T>(string tableName)
        {
            return _database.GetCollection<T>(tableName);
        }

        #endregion Methods
    }
}
