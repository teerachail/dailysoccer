using ApiApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ApiApp.MongoAccess
{
    static class MongoUtil
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private static MongoDbHelper<Team> _team;
        private static MongoDbHelper<Match> _match;
        private static MongoDbHelper<League> _league;
        private static MongoDbHelper<Reward> _reward;
        private static MongoDbHelper<Winner> _winner;
        private static MongoDbHelper<Prediction> _prediction;
        private static MongoDbHelper<UserProfile> _userProfile;
        private static MongoDbHelper<RewardGroup> _rewardGroup;
        private static MongoDbHelper<PendingWinner> _pendingWinner;
        private static MongoDbHelper<FacebookAccount> _facebookAccount;

        static MongoUtil()
        {
            var connectionString = WebConfigurationManager.AppSettings["primaryConnectionString"];
            _client = new MongoClient(connectionString);

            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            _database = _client.GetDatabase(dbName);

            _team = new MongoDbHelper<Team>(_database, "dailysoccer.Teams");
            _match = new MongoDbHelper<Match>(_database, "dailysoccer.Matches");
            _league = new MongoDbHelper<League>(_database, "dailysoccer.Leagues");
            //_reward = new MongoDbHelper<Reward>(_database, "dailysoccer.Rewards");
            //_winner = new MongoDbHelper<Winner>(_database, "dailysoccer.Winners");
            _prediction = new MongoDbHelper<Prediction>(_database, "dailysoccer.Predictions");
            //_userProfile = new MongoDbHelper<UserProfile>(_database, "dailysoccer.UserProfiles");
            //_rewardGroup = new MongoDbHelper<RewardGroup>(_database, "dailysoccer.RewardGroups");
            _pendingWinner = new MongoDbHelper<PendingWinner>(_database, "dailysoccer.PendingWinners");
            //_facebookAccount = new MongoDbHelper<FacebookAccount>(_database, "dailysoccer.FacebookAccounts");
        }

        /// <summary>
        /// ดึงข้อมูลจากตาราง
        /// </summary>
        /// <typeparam name="T">ข้อมูลที่ทำงานด้วย</typeparam>
        /// <param name="tableName">ชื่อตาราง</param>
        public static IMongoCollection<T> GetCollection<T>(string tableName)
        {
            return _database.GetCollection<T>(tableName);
        }

        /// <summary>
        /// ดึงประวัติการทายผลของผู้ใช้
        /// </summary>
        public static IEnumerable<Prediction> GetUserPredictions()
        {
            return _prediction.Read();
        }

        /// <summary>
        /// ดึงแมช์การแข่งขันทั้งหมดในระบบ
        /// </summary>
        public static IEnumerable<Match> GetMatches()
        {
            return _match.Read();
        }

        /// <summary>
        /// ดึงทีมทั้งหมดในระบบ
        /// </summary>
        public static IEnumerable<Team> GetTeams()
        {
            return _team.Read();
        }

        /// <summary>
        /// ดึงลีกทั้งหมดในระบบ
        /// </summary>
        public static IEnumerable<League> GetAllLeagues()
        {
            return _league.Read();
        }

        /// <summary>
        /// กำหนดการทายผลของผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการกำหนด</param>
        /// <param name="matchId">รหัสแมช์การแข่งขันที่ผู้ใช้เลือก</param>
        /// <param name="winnerTeamId">รหัสทีมที่ผู้ใช้เลือก</param>
        /// <param name="predictionPoints">คะแนนที่ผู้ใช้จะได้ถ้าทายถูก</param>
        /// <param name="currentTime">วันเวลาที่ทำการบันทึกข้อมูล</param>
        public static void SetUserPrediction(string userId, string matchId, string winnerTeamId, int predictionPoints, DateTime currentTime)
        {
            var predictionId = string.Format("{0}-{1}", userId, matchId);
            var lastPrediction = _prediction.Read().FirstOrDefault(it => it.id.Equals(predictionId));
            if (lastPrediction == null)
            {
                _prediction.Create(new Prediction
                {
                    id = predictionId,
                    PredictionTeamId = winnerTeamId,
                    PredictionPoints = predictionPoints,
                    CreatedDate = currentTime
                });
            }
            else
            {
                _prediction.Update(predictionId, it => it.PredictionTeamId, winnerTeamId);
                _prediction.Update(predictionId, it => it.PredictionPoints, predictionPoints);
                _prediction.Update(predictionId, it => it.CreatedDate, currentTime);
            }
        }

        /// <summary>
        /// ยกเลิกการทายผลของผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการกำหนด</param>
        /// <param name="matchId">รหัสแมช์การแข่งขันที่ผู้ใช้ยกเลิก</param>
        public static void CancelUserPrediction(string userId, string matchId)
        {
            var deleteId = string.Format("{0}-{1}", userId, matchId);
            _prediction.Delete(it => it.id, deleteId);
        }
    }
}
