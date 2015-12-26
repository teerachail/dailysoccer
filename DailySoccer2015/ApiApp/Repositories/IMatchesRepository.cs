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
        /// ดึงแมช์การแข่งขันจากวันที่
        /// </summary>
        /// <param name="date">วันที่ที่ต้องการดึงข้อมูล</param>
        IEnumerable<MatchInformation> GetMatchesByDate(DateTime date);

        #endregion Methods
    }
}
