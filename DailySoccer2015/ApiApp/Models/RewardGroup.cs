using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// กลุ่มของรางวัล
    /// </summary>
    public class RewardGroup
    {
        #region Properties

        /// <summary>
        /// รหัสกลุ่มของรางวัล
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// วันหมดอายุของกลุ่ม
        /// </summary>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        /// จำนวนแต้มที่ต้องการต่อหนึ่งคูปอง
        /// </summary>
        public int RequiredPoints { get; set; }

        #endregion Properties
    }
}
