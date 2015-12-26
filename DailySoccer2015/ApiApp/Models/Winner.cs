using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ผู้โชคดี
    /// </summary>
    public class Winner
    {
        #region Properties

        /// <summary>
        /// รหัสผู้โชคดี
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// รหัสของรางวัลที่ได้
        /// </summary>
        public string RewardId { get; set; }

        /// <summary>
        /// รหัสบัญชีผู้ใช้ที่ได้รางวัล
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// รหัสอ้างอิงในการติดต่อรับของรางวัล
        /// </summary>
        public string ReferenceCode { get; set; }

        /// <summary>
        /// ติดต่อผู้ใช้ไปแล้วหรือยัง?
        /// </summary>
        public bool IsAlreadyContact { get; set; }

        /// <summary>
        /// วันเวลาที่ถูกจับรางวัล
        /// </summary>
        public DateTime CreatedDate { get; set; }

        #endregion Properties
    }
}
