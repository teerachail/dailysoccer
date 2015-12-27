using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับแมช์การแข่งขัน
    /// </summary>
    public interface IMatchesRepository
    {
        #region Methods

        /// <summary>
        /// ดึงแมช์การแข่งขันทั้งหมดในระบบ
        /// </summary>
        IEnumerable<Match> GetMatches();

        /// <summary>
        /// ดึงลีกทั้งหมดในระบบ
        /// </summary>
        IEnumerable<League> GetAllLeagues();

        /// <summary>
        /// ดึงทีมทั้งหมดในระบบ
        /// </summary>
        IEnumerable<Team> GetTeams();

        #endregion Methods
    }
}
