using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลการทายผลแมช์การแข่งขัน
    /// </summary>
    public class PredictionRequest
    {
        #region Properties

        /// <summary>
        /// รหัสแมช์การแข่งขันที่เลือก
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// รหัสทีมที่ทาย (empty = draw)
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// เป็นการยกเลิกการทายผลที่เคยทายไว้หรือไม่
        /// </summary>
        public bool IsCancel { get; set; }

        #endregion Properties
    }
}
