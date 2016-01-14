using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ลีก
    /// </summary>
    public class League
    {
        #region Properties

        /// <summary>
        /// รหัสลีก
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// ชื่อลีก
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ช่วงเวลาที่แตกต่างกัน
        /// </summary>
        public int DifferentDay { get; set; }

        #endregion Properties
    }
}
