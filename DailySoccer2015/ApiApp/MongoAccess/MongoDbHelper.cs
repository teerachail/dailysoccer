using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.MongoAccess
{
    /// <summary>
    /// Helper for MongoDb
    /// </summary>
    /// <typeparam name="T">Table's structure</typeparam>
    public class MongoDbHelper<T>
    {
        private IMongoDatabase _database;
        private string _tableName;

        /// <summary>
        /// Initializes helper
        /// </summary>
        /// <param name="db">ตัวติดต่อ MongoDb</param>
        /// <param name="tableName">ชื่อตารางที่ทำงานด้วย</param>
        public MongoDbHelper(IMongoDatabase db, string tableName)
        {
            _database = db;
            _tableName = tableName;
        }

        /// <summary>
        /// อ่านข้อมูลทั้งหมดของตาราง
        /// </summary>
        public IEnumerable<T> Read()
        {
            return _database.GetCollection<T>(_tableName).Find(it => true).ToList();
        }

        /// <summary>
        /// เพิ่มข้อมูลเข้าตาราง
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการจะเพิ่ม</param>
        public void Create(T data)
        {
            _database.GetCollection<T>(_tableName).InsertOne(data);
        }
        /// <summary>
        /// เพิ่มข้อมูลเข้าตารางเป็นกลุ่ม
        /// </summary>
        /// <param name="data">กลุ่มข้อมูลที่ต้องการจะเพิ่ม</param>
        public void Create(IEnumerable<T> data)
        {
            _database.GetCollection<T>(_tableName).InsertMany(data);
        }
        /// <summary>
        /// เพิ่มข้อมูลเข้าตารางเป็นกลุ่ม
        /// </summary>
        /// <param name="data">กลุ่มข้อมูลที่ต้องการจะเพิ่ม</param>
        public void Create(List<T> data)
        {
            _database.GetCollection<T>(_tableName).InsertMany(data);
        }

        /// <summary>
        /// อัพเดทข้อมูลในตาราง
        /// </summary>
        /// <param name="id">key ของข้อมูลที่ต้องการอัพเดท</param>
        /// <param name="fnUpdate">field ที่ต้องการอัพเดท</param>
        /// <param name="newValue">ข้อมูลที่ใช้เปลี่ยน</param>
        public void Update<TField>(string id, Expression<Func<T, TField>> fnUpdate, TField newValue)
        {
            var filter = Builders<T>.Filter.Eq("id", id);
            var update = Builders<T>.Update.Set(fnUpdate, newValue);
            _database.GetCollection<T>(_tableName).UpdateOne(filter, update);
        }

        /// <summary>
        /// ลบข้อมูล
        /// </summary>
        /// <param name="condition">field ที่ใช้เป็นเงื่อนไขในการลบข้อมูล</param>
        /// <param name="conditionValue">ข้อมูลที่ใช้ตรวจสอบการลบข้อมูล</param>
        public void Delete<TField>(Expression<Func<T, TField>> condition, TField conditionValue)
        {
            var filter = Builders<T>.Filter.Eq(condition, conditionValue);
            _database.GetCollection<T>(_tableName).DeleteOne(filter);
        }

        /// <summary>
        /// ลบข้อมูลทั้งหมดในตาราง
        /// </summary>
        public void Clear()
        {
            _database.GetCollection<T>(_tableName).DeleteMany(it => true);
        }
    }
}
