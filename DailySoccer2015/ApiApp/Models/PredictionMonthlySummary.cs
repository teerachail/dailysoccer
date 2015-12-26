using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ประวัติการทายผลรายเดือน
    /// </summary>
    public class PredictionMonthlySummary
    {
        #region Properties

        /// <summary>
        /// เดือน
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// คะแนนที่ได้ทั้งหมดภายในเดือน
        /// </summary>
        public int TotalPoints { get; set; }

        #endregion Properties
    }
}
