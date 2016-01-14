using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการเชื่อมต่อกับฟุตบอลเซอร์วิส
    /// </summary>
    public interface IFootballService
    {
        #region Methods

        /// <summary>
        /// ดึงข้อมูลแมช์การแข่งขันจากรหัสลีก
        /// </summary>
        /// <param name="leagueId">รหัสลีก</param>
        /// <param name="fromDate">เริ่มดึงจากวันที่</param>
        /// <param name="toDate">ดึงถึงวันที่</param>
        IEnumerable<MatchAPIInformation> GetMatchesByLeagueId(string leagueId, DateTime fromDate, DateTime toDate);

        #endregion Methods
    }
}
