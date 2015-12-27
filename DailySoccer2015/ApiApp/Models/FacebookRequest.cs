using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลการขอข้อมูลผ่านทาง Facebook
    /// </summary>
    public class FacebookRequest
    {
        #region Properties

        /// <summary>
        /// รหัสบัญชี Facebook
        /// </summary>
        public string FacebookId { get; set; }

        /// <summary>
        /// รหัสบัญชีผู้ใช้
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// เป็นการยืนยันการผูกบัญชีผู้ใช้กับบัญชี Facebook หรือไม่?
        /// </summary>
        public bool IsConfirmed { get; set; }

        #endregion Properties
    }
}
