using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ของรางวัลของฉัน
    /// </summary>
    public class MyReward
    {
        #region Properties

        /// <summary>
        /// รหัสของรางวัล
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// ของรางวัลประจำวันที่
        /// </summary>
        public DateTime RewardDate { get; set; }

        /// <summary>
        /// ลำดับที่
        /// </summary>
        public int OrderedNo { get; set; }

        /// <summary>
        /// มูลค่า
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// รูปของรางวัลขนาดเล็ก
        /// </summary>
        public string ThumbImgPath { get; set; }

        /// <summary>
        /// รายละเอียด
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// รหัสอ้างอิง
        /// </summary>
        public string ReferenceCode { get; set; }

        /// <summary>
        /// เป็นของรางวัลในปัจจุบันหรือไม่
        /// </summary>
        public bool IsPresent { get; set; }

        #endregion Properties
    }
}
