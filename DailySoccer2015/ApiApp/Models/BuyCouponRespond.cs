using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ผลการซื้อคูปอง
    /// </summary>
    public class BuyCouponRespond
    {
        #region Properties

        /// <summary>
        /// การสั่งซื้อสำเร็จหรือไม่
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// รายละเอียดข้อผิดพลาด
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// วันที่ประกาศผลรางวัล
        /// </summary>
        public DateTime AnnounceableDate { get; set; }

        #endregion Properties
    }
}
