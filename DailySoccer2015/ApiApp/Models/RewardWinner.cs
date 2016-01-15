using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ของรางวัลและผู้ชนะ
    /// </summary>
    public class RewardWinner
    {
        #region Properties

        /// <summary>
        /// รหัสของรางวัล
        /// </summary>
        public string id { get; set; }

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

        /// <summary>
        /// รายชื่อผู้โชคดี
        /// </summary>
        public IEnumerable<DisplayWinner> Winners { get; set; }

        #endregion Properties
    }
}
