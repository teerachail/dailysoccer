using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลการซื้อคูปอง
    /// </summary>
    public class BuyCouponRequest
    {
        #region Properties

        /// <summary>
        /// รหัสผู้ใช้ที่ทำการสั่งซื้อ
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// จำนวนคูปองที่ต้องการจะซื้อ
        /// </summary>
        public int BuyAmount { get; set; }

        #endregion Properties
    }
}
