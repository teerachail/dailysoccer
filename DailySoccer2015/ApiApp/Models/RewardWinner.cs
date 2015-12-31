using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ของรางวัลและผู้ชนะ
    /// </summary>
    public class RewardWinner
    {
        #region Properties

        /// <summary>
        /// รหัสของรางวัล
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// รายชื่อผู้โชคดี
        /// </summary>
        public IEnumerable<string> Winners { get; set; }

        #endregion Properties
    }
}
