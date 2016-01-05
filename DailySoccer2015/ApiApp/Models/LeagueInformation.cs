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
    /// ลีก
    /// </summary>
    public class LeagueInformation
    {
        #region Properties

        /// <summary>
        /// ชื่อลีก
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// รายการแข่ง
        /// </summary>
        public IEnumerable<MatchInformation> Matches { get; set; }

        #endregion Properties
    }
}
