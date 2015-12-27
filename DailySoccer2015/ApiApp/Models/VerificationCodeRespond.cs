using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ผลการขอยืนยันเบอร์โทรศัพท์
    /// </summary>
    public class VerificationCodeRespond
    {
        #region Properties

        /// <summary>
        /// สถานะการยืนยันสำเร็จหรือไม่?
        /// </summary>
        public bool IsSuccess { get; set; }

        #endregion Properties
    }
}
