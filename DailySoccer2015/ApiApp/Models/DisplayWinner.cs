using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// การแสดงผลผู้โชคดี
    /// </summary>
    public class DisplayWinner
    {
        #region Properties

        /// <summary>
        /// ลำดับที่
        /// </summary>
        public int Ordering { get; set; }

        /// <summary>
        /// ชื่อผู้โชคดี
        /// </summary>
        public string DisplayWinnerName { get; set; }

        #endregion Properties
    }
}
