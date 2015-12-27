using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการจัดการตัวส่ง SMS
    /// </summary>
    public interface ISMSSender
    {
        #region Methods

        /// <summary>
        /// ส่ง SMS
        /// </summary>
        /// <param name="sentToPhoneNumber">เบอร์ปลายทาง</param>
        /// <param name="message">ข้อความ</param>
        void Send(string sentToPhoneNumber, string message);

        #endregion Methods
    }
}
