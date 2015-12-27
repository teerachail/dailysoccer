using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Twilio;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวส่ง SMS จาก Twilio
    /// </summary>
    public class TwilioSMSSender : ISMSSender
    {
        #region ISMSSender members

        /// <summary>
        /// ส่ง SMS
        /// </summary>
        /// <param name="sentToPhoneNumber">เบอร์ปลายทาง</param>
        /// <param name="message">ข้อความ</param>
        public void Send(string sentToPhoneNumber, string message)
        {
            var accountSID = WebConfigurationManager.AppSettings["twilioAccountSID"];
            var authToken = WebConfigurationManager.AppSettings["twilioAuthToken"];
            var client = new TwilioRestClient(accountSID, authToken);

            var SentFromPhoneNumber = WebConfigurationManager.AppSettings["twilioPrimaryPhoneNo"];
            var result = client.SendMessage(SentFromPhoneNumber, sentToPhoneNumber, message);

            if (result.RestException != null)
            {
                string errorMessage = result.RestException.Message;
            }
        }

        #endregion ISMSSender members
    }
}
