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
    public class MatchInformation
    {
        #region Properties

        /// <summary>
        /// รหัสแมช์การแข่งขัน
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// รหัสทีมเจ้าบ้าน
        /// </summary>
        public string TeamHomeId { get; set; }

        /// <summary>
        /// ชื่อทีมเจ้าบ้าน
        /// </summary>
        public string TeamHomeName { get; set; }

        /// <summary>
        /// คะแนนทีมเจ้าบ้าน
        /// </summary>
        public int TeamHomeScore { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลทีมเจ้าบ้าน
        /// </summary>
        public int TeamHomePoint { get; set; }

        /// <summary>
        /// รหัสทีมเยือน
        /// </summary>
        public string TeamAwayId { get; set; }

        /// <summary>
        /// ชื่อทีมเยือน
        /// </summary>
        public string TeamAwayName { get; set; }

        /// <summary>
        /// คะแนนทีมเยือน
        /// </summary>
        public int TeamAwayScore { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลทีมเยือน
        /// </summary>
        public int TeamAwayPoint { get; set; }

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
        /// ชื่อลีค
        /// </summary>
        public string LeagueName { get; set; }

        /// <summary>
        /// สถานะแมทช์
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลเสมอ
        /// </summary>
        public int DrawPoints { get; set; }

        /// <summary>
        /// ผลการแข่งทีมเหย้าชนะ
        /// </summary>
        public bool IsTeamHomeWin { get
            {
                if (CompletedDate.HasValue)
                {
                    return TeamHomeScore > TeamAwayScore;
                }
                return false;
            }}

        /// <summary>
        /// ผลการแข่งทีมเยือนชนะ
        /// </summary>
        public bool IsTeamAwayWin
        {
            get
            {
                if (CompletedDate.HasValue)
                {
                    return TeamAwayScore > TeamHomeScore;
                }
                return false;
            }
        }

        /// <summary>
        /// ผลการแข่งเสมอ
        /// </summary>
        public bool IsGameDraw
        {
            get
            {
                if (CompletedDate.HasValue)
                {
                    return TeamAwayScore == TeamHomeScore;
                }
                return false;
            }
        }

        #endregion Properties
    }
}
