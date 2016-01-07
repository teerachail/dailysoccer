using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// รายละเอียดประวัติการทายผลรายวัน
    /// </summary>
    public class PredictionDailyDetail
    {
        #region Properties

        /// <summary>
        /// ชื่อทีมเจ้าบ้าน
        /// </summary>
        public string TeamHomeName { get; set; }

        /// <summary>
        /// ชื่อทีมเยือน
        /// </summary>
        public string TeamAwayName { get; set; }

        /// <summary>
        /// คะแนนเจ้าบ้าน
        /// </summary>
        public int TeamHomeScore { get; set; }

        /// <summary>
        /// คะแนนทีมเยือน
        /// </summary>
        public int TeamAwayScore { get; set; }

        /// <summary>
        /// แมช์การแข่งขันจบแล้วหรือยัง
        /// </summary>
        public bool IsMatchFinish { get; set; }

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
        /// คะแนนที่ได้
        /// </summary>
        public int GainPoints { get; set; }

        #endregion Properties
    }
}
