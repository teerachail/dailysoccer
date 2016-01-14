using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiApp.MongoAccess;
using MongoDB.Driver;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับแมช์การแข่งขัน
    /// </summary>
    public class MatchesRepository : IMatchesRepository
    {
        #region Fields

        private const string MatchTableName = "dailysoccer.Matches";
        private const string TeamTableName = "dailysoccer.Teams";
        private const string LeagueTableName = "dailysoccer.Leagues";

        #endregion Fields

        #region IMatchesRepository members

        /// <summary>
        /// ดึงแมช์การแข่งขันทั้งหมดในระบบ
        /// </summary>
        public IEnumerable<Match> GetAllMatches()
        {
            var qry = MongoUtil.GetCollection<Match>(MatchTableName)
                .Find(it => true)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงแมช์การแข่งขันจากวันที่แข่งขัน
        /// </summary>
        /// <param name="beginDate">วันที่แข่งขันที่ต้องการดึงข้อมูล</param>
        public IEnumerable<Match> GetMatchesByDate(DateTime beginDate)
        {
            var qry = MongoUtil.GetCollection<Match>(MatchTableName)
               .Find(it => true)
               .ToEnumerable()
               .Where(it => it.BeginDate.Date == beginDate.Date);
            return qry;
        }

        /// <summary>
        /// ดึงแมช์การแข่งขันจากรหัสการแข่งขัน
        /// </summary>
        /// <param name="matchId">รหัสการแข่งขันที่ต้องการดึงข้อมูล</param>
        public Match GetMatchById(string matchId)
        {
            var selectedMatch = MongoUtil.GetCollection<Match>(MatchTableName)
                .Find(it => it.id.Equals(matchId))
                .FirstOrDefault();
            return selectedMatch;
        }

        /// <summary>
        /// ดึงแมช์การแข่งขันจากปี
        /// </summary>
        /// <param name="year">ปีที่ต้องการดึงข้อมูล</param>
        public IEnumerable<Match> GetMatchesByYear(int year)
        {
            var qry = MongoUtil.GetCollection<Match>(MatchTableName)
                .Find(it => true)
                .ToEnumerable()
                .Where(it => it.BeginDate.Year == year);
            return qry;
        }

        /// <summary>
        /// ดึงลีกทั้งหมดในระบบ
        /// </summary>
        public IEnumerable<League> GetAllLeagues()
        {
            var qry = MongoUtil.GetCollection<League>(LeagueTableName)
                .Find(it => true)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงรายการลีกจากรหัสลีก
        /// </summary>
        /// <param name="leagueIds">รหัสลีกที่ต้องการดึง</param>
        public IEnumerable<League> GetLeaguesByIds(IEnumerable<string> leagueIds)
        {
            var qry = MongoUtil.GetCollection<League>(LeagueTableName)
                .Find(it => leagueIds.Contains(it.id))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงรายการทีมจากรหัสลีก
        /// </summary>
        /// <param name="leagueId">รหัสลีกที่ต้องการดึงข้อมูล</param>
        public IEnumerable<Team> GetTeamsByLeagueId(string leagueId)
        {
            var qry = MongoUtil.GetCollection<Team>(TeamTableName)
                .Find(it => it.LeagueId.Equals(leagueId))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงรายการทีมจากรหัสทีม
        /// </summary>
        /// <param name="teamIds">รหัสทีมที่ต้องการดึงข้อมูล</param>
        public IEnumerable<Team> GetTeamsByIds(IEnumerable<string> teamIds)
        {
            var qry = MongoUtil.GetCollection<Team>(TeamTableName)
                .Find(it => teamIds.Contains(it.id))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงทีมจากรหัสทีม
        /// </summary>
        public Team GetTeamById(string teamId)
        {
            var selectedTeam = MongoUtil.GetCollection<Team>(TeamTableName)
                .Find(it => it.id.Equals(teamId))
                .FirstOrDefault();
            return selectedTeam;
        }

        /// <summary>
        /// ดึงแมช์การแข่งขันที่ยังไม่จบ
        /// </summary>
        public IEnumerable<Match> GetMatchesUnComplete()
        {
            return new List<Match>();
        }

        #endregion IMatchesRepository members
    }
}
