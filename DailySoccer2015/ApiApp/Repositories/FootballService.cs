using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiApp.Models;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวเชื่อมต่อกับฟุตบอลเซอร์วิส
    /// </summary>
    public class FootballService : IFootballService
    {
        #region IFootballService members

        /// <summary>
        /// ดึงข้อมูลแมช์การแข่งขันจากรหัสลีก
        /// </summary>
        /// <param name="leagueId">รหัสลีก</param>
        public IEnumerable<MatchAPIInformation> GetMatchesByLeagueId(string leagueId)
        {
            // TODO: Not implement
            throw new NotImplementedException();
        }

        #endregion IFootballService members
    }
}
