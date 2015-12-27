using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับแมช์การแข่งขัน
    /// </summary>
    public class MatchesRepository : IMatchesRepository
    {
        #region IMatchesRepository members

        /// <summary>
        /// ดึงแมช์การแข่งขันทั้งหมดในระบบ
        /// </summary>
        public IEnumerable<Match> GetMatches()
        {
            return MongoAccess.MongoUtil.GetMatches();
        }

        /// <summary>
        /// ดึงลีกทั้งหมดในระบบ
        /// </summary>
        public IEnumerable<League> GetAllLeagues()
        {
            return MongoAccess.MongoUtil.GetAllLeagues();
        }

        /// <summary>
        /// ดึงทีมทั้งหมดในระบบ
        /// </summary>
        public IEnumerable<Team> GetTeams()
        {
            return MongoAccess.MongoUtil.GetTeams();
        }

        #endregion IMatchesRepository members
    }
}
