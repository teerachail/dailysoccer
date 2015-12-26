using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// บัญชี Facebook
    /// </summary>
    public class FacebookAccount
    {
        #region Properties

        /// <summary>
        /// รหัสอ้างอิง Facebook
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// รหัสบัญชีผู้ใช้
        /// </summary>
        public string UserId { get; set; }

        #endregion Properties
    }
}
