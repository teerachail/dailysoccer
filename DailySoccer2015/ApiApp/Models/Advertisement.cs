using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลของโฆษณา
    /// </summary>
    public class Advertisement
    {
        #region Properties

        /// <summary>
        /// รหัสโฆษณา
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// ที่อยู่ของรูปโฆษณา
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// ลิงค์ของโฆษณา
        /// </summary>
        public string LinkUrl { get; set; }

        #endregion Properties
    }
}
