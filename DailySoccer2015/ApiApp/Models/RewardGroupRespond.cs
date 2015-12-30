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
    public class RewardGroupRespond
    {
        #region Properties

        /// <summary>
        /// พร้อมเปิดให้ซื้อหรือยัง
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// วันหมดอายุของกลุ่ม
        /// </summary>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        /// จำนวนแต้มที่ต้องการต่อหนึ่งคูปอง
        /// </summary>
        public int RequiredPoints { get; set; }

        /// <summary>
        /// รายการของรางวัล
        /// </summary>
        public IEnumerable<Reward> Rewards { get; set; }

        #endregion Properties
    }
}
