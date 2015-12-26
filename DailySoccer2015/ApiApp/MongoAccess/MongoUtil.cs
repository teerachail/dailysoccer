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
        private static MongoDbHelper<Team> _team = new MongoDbHelper<Team>(_database, "dailysoccer.Teams");
        private static MongoDbHelper<Match> _match = new MongoDbHelper<Match>(_database, "dailysoccer.Matches");
        private static MongoDbHelper<League> _league = new MongoDbHelper<League>(_database, "dailysoccer.Leagues");
        private static MongoDbHelper<Reward> _reward = new MongoDbHelper<Reward>(_database, "dailysoccer.Rewards");
        private static MongoDbHelper<Winner> _winner = new MongoDbHelper<Winner>(_database, "dailysoccer.Winners");
        private static MongoDbHelper<Prediction> _prediction = new MongoDbHelper<Prediction>(_database, "dailysoccer.Predictions");
        private static MongoDbHelper<UserProfile> _userProfile = new MongoDbHelper<UserProfile>(_database, "dailysoccer.UserProfiles");
        private static MongoDbHelper<RewardGroup> _rewardGroup = new MongoDbHelper<RewardGroup>(_database, "dailysoccer.RewardGroups");
        private static MongoDbHelper<PendingWinner> _pendingWinner = new MongoDbHelper<PendingWinner>(_database, "dailysoccer.PendingWinners");
        private static MongoDbHelper<FacebookAccount> _facebookAccount = new MongoDbHelper<FacebookAccount>(_database, "dailysoccer.FacebookAccounts");

        static MongoUtil()
        {
            var connectionString = WebConfigurationManager.AppSettings["primaryConnectionString"];
            _client = new MongoClient(connectionString);

            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            _database = _client.GetDatabase(dbName);
        }

        public static string /*MongoCollection*/ GetCollection(string collectionName)
        {
            //var connectionString = WebConfigurationManager.ConnectionStrings["primaryConnectionString"].ConnectionString;
            var dbName = WebConfigurationManager.AppSettings["databaseName"];
            return null;
        }
    }
}
