using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลทีมที่ชอบ
    /// </summary>
    public class SetFavoriteTeamRequest
    {
        #region Properties

        /// <summary>
        /// รหัสทีมที่ชอบ
        /// </summary>
        public string TeamId { get; set; }

        #endregion Properties
    }
}
