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
    /// แมช์การแข่งขัน
    /// </summary>
    public class Match
    {
        #region Properties

        /// <summary>
        /// รหัสแมช์การแข่งขัน
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// รหัสทีมเจ้าบ้าน
        /// </summary>
        public string TeamHomeId { get; set; }

        /// <summary>
        /// คะแนนทีมเจ้าบ้าน
        /// </summary>
        public int TeamHomeScore { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลทีมเจ้าบ้าน
        /// </summary>
        public int? TeamHomePoint { get; set; }

        /// <summary>
        /// รหัสทีมเยือน
        /// </summary>
        public string TeamAwayId { get; set; }

        /// <summary>
        /// คะแนนทีมเยือน
        /// </summary>
        public int TeamAwayScore { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลทีมเยือน
        /// </summary>
        public int? TeamAwayPoint { get; set; }

        /// <summary>
        /// วันเวลาในการแข่งขัน
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// วันเวลาที่แมทช์การแข่งขันเริ่มแข่งจริง
        /// </summary>
        public DateTime? StartedDate { get; set; }

        /// <summary>
        /// วันเวลาที่แมทช์การแข่งขันจบ
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// รหัสลีก
        /// </summary>
        public string LeagueId { get; set; }

        /// <summary>
        /// สถานะแมทช์
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลเสมอ
        /// </summary>
        public int? DrawPoints { get; set; }

        #endregion Properties
    }
}
