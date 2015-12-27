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
            _reward = new MongoDbHelper<Reward>(_database, "dailysoccer.Rewards");
            _winner = new MongoDbHelper<Winner>(_database, "dailysoccer.Winners");
            _prediction = new MongoDbHelper<Prediction>(_database, "dailysoccer.Predictions");
            _userProfile = new MongoDbHelper<UserProfile>(_database, "dailysoccer.UserProfiles");
            _rewardGroup = new MongoDbHelper<RewardGroup>(_database, "dailysoccer.RewardGroups");
            _pendingWinner = new MongoDbHelper<PendingWinner>(_database, "dailysoccer.PendingWinners");
            _facebookAccount = new MongoDbHelper<FacebookAccount>(_database, "dailysoccer.FacebookAccounts");
        }

        /// <summary>
        /// ดึงรายการกลุ่มของรางวัล
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<RewardGroup> GetRewardGroups()
        {
            return _rewardGroup.Read();
        }

        /// <summary>
        /// ดึงรายการของรางวัลจากรหัสกลุ่ม
        /// </summary>
        public static IEnumerable<Reward> GetRewards()
        {
            return _reward.Read();
        }

        public static string /*MongoCollection*/ GetCollection(string collectionName)
        {
            //var connectionString = WebConfigurationManager.ConnectionStrings["primaryConnectionString"].ConnectionString;
            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            return null;
        }
    }
}
