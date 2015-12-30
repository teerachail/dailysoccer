using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลการสั่งซื้อคูปองของผู้ใช้
    /// </summary>
    public class CouponSummaryRespond
    {
        #region Properties

        /// <summary>
        /// คะแนนที่มี
        /// </summary>
        public int RemainingPoints { get; set; }

        /// <summary>
        /// จำนวนคูปองที่ซื้อไว้
        /// </summary>
        public int OrderedCoupons { get; set; }

        #endregion Properties
    }
}
