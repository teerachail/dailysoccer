using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ผู้โชคดีที่กำลังอยู่ในช่วงพิจารณา
    /// </summary>
    public class PendingWinner
    {
        #region Properties

        /// <summary>
        /// รหัสผู้โชคดีที่กำลังอยู่ในช่วงพิจารณา
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// รหัสของรางวัลที่จะได้
        /// </summary>
        public string RewardId { get; set; }

        /// <summary>
        /// ถูกเลือกจากเจ้าหน้าที่?
        /// </summary>
        public bool IsManualSelection { get; set; }

        /// <summary>
        /// วันเวลาที่ถูกรับรองให้นำไปเป็นผู้โชคดีจริงๆ
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        #endregion Properties
    }
}
