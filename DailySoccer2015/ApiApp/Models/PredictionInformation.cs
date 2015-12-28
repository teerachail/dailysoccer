using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลการทายแมช์การแข่งขัน
    /// </summary>
    public class PredictionInformation
    {
        #region Properties
        
        /// <summary>
        /// รหัสแมช์การแข่งขัน
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// ทายผลว่าทีมเจ้าบ้านชนะ
        /// </summary>
        public bool IsPredictionTeamHome { get; set; }

        /// <summary>
        /// ทายผลว่าทีมเยือนชนะ
        /// </summary>
        public bool IsPredictionTeamAway { get; set; }

        /// <summary>
        /// ทายผลว่าแมช์นี้จะเสมอ
        /// </summary>
        public bool IsPredictionDraw { get; set; }

        /// <summary>
        /// คะแนนที่จะได้ถ้าทายถูก
        /// </summary>
        public int PredictionPoints { get; set; }

        #endregion Properties
    }
}
