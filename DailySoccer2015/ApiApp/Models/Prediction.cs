using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// การทายผล
    /// </summary>
    public class Prediction
    {
        #region Properties

        /// <summary>
        /// รหัสการทายผล
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// รหัสทีมที่ทาย (empty = draw)
        /// </summary>
        public string PredictionTeamId { get; set; }

        /// <summary>
        /// รหัสแมทช์ที่ทาย
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// วันเวลาที่แมทช์ที่ทายผลนี้จบ
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// ผลคะแนนที่ได้จากการทาย
        /// </summary>
        public int ActualPoints { get; set; }

        /// <summary>
        /// คะแนนที่จะได้ถ้าทายถูก
        /// </summary>
        public int PredictionPoints { get; set; }

        /// <summary>
        /// วันเวลาที่ทำการทายผล
        /// </summary>
        public DateTime CreatedDate { get; set; }

        #endregion Properties
    }
}
