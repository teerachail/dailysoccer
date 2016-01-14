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
        /// อัพเดทหรือเพิ่มแมช์ใหม่
        /// </summary>
        /// <param name="match">ข้อมูลแมช์ที่จะดำเนินการ</param>
        public void UpsertMatch(Match match)
        {
            var update = Builders<Match>.Update
              .Set(it => it.BeginDate, match.BeginDate)
              .Set(it => it.CompletedDate, match.CompletedDate)
              .Set(it => it.LeagueId, match.LeagueId)
              .Set(it => it.StartedDate, match.StartedDate)
              .Set(it => it.Status, match.Status)
              .Set(it => it.TeamAwayId, match.TeamAwayId)
              .Set(it => it.TeamAwayScore, match.TeamAwayScore)
              .Set(it => it.TeamHomeId, match.TeamHomeId)
              .Set(it => it.TeamHomeScore, match.TeamHomeScore)
              //.Set(it => it.DrawPoints, match.DrawPoints)
              //.Set(it => it.TeamAwayPoint, match.TeamAwayPoint)
              //.Set(it => it.TeamHomePoint, match.TeamHomePoint)
              .Set(it => it.TeamHomeName, match.TeamHomeName)
              .Set(it => it.TeamAwayName, match.TeamAwayName)
              .Set(it => it.LeagueName, match.LeagueName)
              .Set(it => it.BeginDateTimeUTD, match.BeginDateTimeUTD)
              .Set(it => it.FilterDateTime, match.FilterDateTime)
              .Set(it => it.GameMinutes, match.GameMinutes)
              .Set(it => it.LastCalculatedDateTime, match.LastCalculatedDateTime)
              .Set(it => it.CreatedDateTime, match.CreatedDateTime);
            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoUtil.GetCollection<Match>(MatchTableName)
                .UpdateOne(it => it.id == match.id, update, updateOption);
        }

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
        /// ดึงแมช์การแข่งขันจากรหัสการแข่งขัน
        /// </summary>
        /// <param name="matchIds">รหัสการแข่งขันที่ต้องการดึงข้อมูล</param>
        public IEnumerable<Match> GetMatchById(IEnumerable<string> matchIds)
        {
            var qry = MongoUtil.GetCollection<Match>(MatchTableName)
                .Find(it => matchIds.Contains(it.id))
                .ToEnumerable();
            return qry;
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
        /// ดึงแมช์ที่ยังไม่แจ้งเตือน
        /// </summary>
        public IEnumerable<Match> GetUnNotifyMatches()
        {
            var qry = MongoUtil.GetCollection<Match>(TeamTableName)
               .Find(it => true)
               .ToEnumerable()
               .Where(it => it.LastUpdateDateTime > it.NotifyDateTime)
               .ToList();
            return qry;
        }

        #endregion IMatchesRepository members
    }
}
