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
        /// ดึงแมช์การแข่งขันจากวันที่
        /// </summary>
        /// <param name="date">วันที่ที่ต้องการดึงข้อมูล</param>
        public IEnumerable<MatchInformation> GetMatchesByDate(DateTime date)
        {
            // TODO: GetMatchesByDate
            throw new NotImplementedException();
        }
        
        #endregion IMatchesRepository members
    }
}
