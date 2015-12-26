using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ทีม
    /// </summary>
    public class Team
    {
        #region Properties

        /// <summary>
        /// รหัสทีม
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// ชื่อทีม
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// รหัสลีก
        /// </summary>
        public int LeagueId { get; set; }

        #endregion Properties
    }
}
