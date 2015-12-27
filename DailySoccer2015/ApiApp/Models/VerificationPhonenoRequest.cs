using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลการขอยืนยันเบอร์โทรศัพท์
    /// </summary>
    public class VerificationPhonenoRequest
    {
        #region Properties

        /// <summary>
        /// เบอร์โทรศัพท์ที่ขอทำการยืนยัน
        /// </summary>
        public string PhoneNumber { get; set; }

        #endregion Properties
    }
}
