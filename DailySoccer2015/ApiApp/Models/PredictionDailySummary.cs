using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ประวัติการทายผลรายวัน
    /// </summary>
    public class PredictionDailySummary
    {
        #region Properties

        /// <summary>
        /// วัน
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// คะแนนที่ได้ทั้งหมดภายในเดือน
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// ข้อมูลการทายผลทั้งหมดภายในวัน
        /// </summary>
        public IEnumerable<PredictionDailyDetail> PredictionResults { get; set; }

        #endregion Properties
    }
}
