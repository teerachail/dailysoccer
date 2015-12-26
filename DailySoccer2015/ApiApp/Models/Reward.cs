using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ของรางวัล
    /// </summary>
    public class Reward
    {
        #region Properties

        /// <summary>
        /// รหัสของรางวัล
        /// </summary>
        [BsonId]
        public int id { get; set; }

        /// <summary>
        /// รหัสกลุ่มของรางวัล
        /// </summary>
        public string RewardGroupId { get; set; }

        /// <summary>
        /// ลำดับที่
        /// </summary>
        public int OrderedNo { get; set; }

        /// <summary>
        /// มูลค่า
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// จำนวนรางวัล
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// รูปของรางวัล
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// รูปของรางวัลขนาดเล็ก
        /// </summary>
        public string ThumbImgPath { get; set; }

        /// <summary>
        /// รายละเอียด
        /// </summary>
        public string Description { get; set; }

        #endregion Properties
    }
}
