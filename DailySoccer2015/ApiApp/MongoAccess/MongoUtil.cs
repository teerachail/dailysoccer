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

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะ
        /// </summary>
        public static IEnumerable<Winner> GetWinners()
        {
            return _winner.Read();
        }

        /// <summary>
        /// ดึงรายการบัญชีผู้ใช้
        /// </summary>
        public static IEnumerable<UserProfile> GetUserProfiles()
        {
            return _userProfile.Read();
        }

        /// <summary>
        /// อัพเดทบัญชีผู้ใช้จากการสั่งซื้อคูปอง
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการอัพเดท</param>
        /// <param name="remainingPoints">จำนวนแต้มที่เหลือ</param>
        /// <param name="orderedCoupons">จำนวนคูปองที่สั่งซื้อไปแล้ว</param>
        public static void UpdateFromBuyCoupons(string userId, int remainingPoints, int orderedCoupons)
        {
            var userProfile = _userProfile.Read().FirstOrDefault(it => it.id.Equals(userId));
            if (userProfile == null) return;

            _userProfile.Update(userProfile.id, it => it.Points, remainingPoints);
            _userProfile.Update(userProfile.id, it => it.OrderedCoupon, orderedCoupons);
        }

        /// <summary>
        /// รีเซ็ตข้อมูลเบอร์โทรศัพท์ที่เคยยืนยันไว้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการรีเซ็ต</param>
        public static void ResetVerifiedPhoneNumber(string userId)
        {
            var userProfile = _userProfile.Read().FirstOrDefault(it => it.id.Equals(userId));
            if (userProfile == null) return;

            _userProfile.Update(userProfile.id, it => it.PhoneNo, string.Empty);
            _userProfile.Update(userProfile.id, it => it.VerifierCode, string.Empty);
            _userProfile.Update(userProfile.id, it => it.VerifiedPhoneDate, null);
        }

        /// <summary>
        /// กำหนดรหัสสำหรับตรวจสอบเบอร์โทรศัพท์
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="phoneNumber">เบอร์โทรศัพท์ที่ใช้ในการยืนยัน</param>
        /// <param name="verifierCode">รหัสสำหรับตรวจสอบเบอร์โทรศัพท์</param>
        public static void SetVerifierPhoneNumber(string userId, string phoneNumber, string verifierCode)
        {
            var userProfile = _userProfile.Read().FirstOrDefault(it => it.id.Equals(userId));
            if (userProfile == null) return;

            _userProfile.Update(userProfile.id, it => it.PhoneNo, phoneNumber);
            _userProfile.Update(userProfile.id, it => it.VerifierCode, verifierCode);
        }

        /// <summary>
        /// กำหนดการยืนยันเบอร์โทรศัพท์เสร็จสิ้น
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="completedDate">วันเวลาที่ทำการยืนยันเสร็จสิ้น</param>
        public static void SetVerifiedPhoneNumberComplete(string userId, DateTime completedDate)
        {
            var userProfile = _userProfile.Read().FirstOrDefault(it => it.id.Equals(userId));
            if (userProfile == null) return;

            _userProfile.Update(userProfile.id, it => it.VerifiedPhoneDate, completedDate);
        }

        /// <summary>
        /// สร้างบัญชีผู้ใช้ใหม่
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการสร้าง</param>
        public static void CreateUserProfile(string userId)
        {
            _userProfile.Create(new UserProfile { id = userId });
        }

        /// <summary>
        /// ดึงบัญชี Facebook
        /// </summary>
        public static IEnumerable<FacebookAccount> GetFacebookAccounts()
        {
            return _facebookAccount.Read();
        }

        /// <summary>
        /// ผูกบัญชี Facebook เข้ากับบัญชีผู้ใช้
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการจะผูก</param>
        /// <param name="userId">บัญชีผู้ใช้ที่จะทำการผูก</param>
        public static void TieFacebookAccount(string facebookId, string userId)
        {
            var userprofile = _userProfile.Read().FirstOrDefault(it => it.id.Equals(userId));
            if (userprofile == null) return;

            _userProfile.Update(userId, it => it.IsFacebookVerified, true);
            _facebookAccount.Create(new FacebookAccount { id = facebookId, UserId = userId });
        }

        /// <summary>
        /// ยกเลิกการผูกบัญชี Facebook
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการยกเลิกการผูก</param>
        public static void UntieFacebookAccount(string facebookId)
        {
            var facebookAccount = _facebookAccount.Read().FirstOrDefault(it => it.id.Equals(facebookId));
            if (facebookAccount == null) return;

            _facebookAccount.Delete(it => it.id, facebookId);
            _userProfile.Update(facebookAccount.UserId, it => it.IsFacebookVerified, false);
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
        /// กำหนดทีมที่ชอบ
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนดทีมที่ชอบ</param>
        /// <param name="teamId">รหัสทีมที่ชอบ</param>
        public static void SetFavoriteTeam(string userId, string teamId)
        {
            var userprofile = _userProfile.Read().FirstOrDefault(it => it.id.Equals(userId));
            if (userprofile == null) return;

            _userProfile.Update(userId, it => it.FavouriteTeamId, teamId);
        }
    }
}
