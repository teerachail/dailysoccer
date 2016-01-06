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
    ///ตัวติดต่อกับบัญชีผู้ใช้
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        #region Fields

        private const string UserProfileTableName = "dailysoccer.UserProfiles";
        private const string FacebookTableName = "dailysoccer.FacebookAccounts";

        #endregion Fields

        #region IAccountRepository members

        /// <summary>
        /// สร้างบัญชีผู้ใช้ใหม่
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการสร้าง</param>
        public void CreateUserProfile(string userId)
        {
            var newProfile = new UserProfile { id = userId };
            MongoUtil.GetCollection<UserProfile>(UserProfileTableName).InsertOne(newProfile);
        }

        /// <summary>
        /// ดึงบัญชีผู้ใช้จากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการดึง</param>
        public UserProfile GetUserProfileById(string userId)
        {
            var selectedUserProfile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName)
                .Find(it => it.id.Equals(userId))
                .FirstOrDefault();
            return selectedUserProfile;
        }

        /// <summary>
        /// ดึงบัญชี Facebook จากรหัสบัญชี facebook
        /// </summary>
        /// <param name="facebookId">รหัสบัญชี facebook ที่ต้องการดึง</param>
        public FacebookAccount GetFacebookAccountsById(string facebookId)
        {
            var selectedFacebookAccount = MongoUtil.GetCollection<FacebookAccount>(FacebookTableName)
                .Find(it => it.id.Equals(facebookId))
                .FirstOrDefault();
            return selectedFacebookAccount;
        }

        /// <summary>
        /// ผูกบัญชี Facebook เข้ากับบัญชีผู้ใช้
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการจะผูก</param>
        /// <param name="userId">บัญชีผู้ใช้ที่จะทำการผูก</param>
        public void TieFacebookAccount(string facebookId, string userId)
        {
            var updateUserProfile = Builders<UserProfile>.Update.Set(it => it.IsFacebookVerified, true);
            var userprofile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName);
            userprofile.UpdateMany(it => it.id.Equals(userId), updateUserProfile);

            var newFacebookAccount = new FacebookAccount { id = facebookId, UserId = userId };
            MongoUtil.GetCollection<FacebookAccount>(FacebookTableName).InsertOne(newFacebookAccount);
        }

        /// <summary>
        /// ยกเลิกการผูกบัญชี Facebook
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการยกเลิกการผูก</param>
        public void UntieFacebookAccount(string facebookId)
        {
            var facebookTable = MongoUtil.GetCollection<FacebookAccount>(FacebookTableName);
            var selectedFacebookAccount = facebookTable
                .Find(it => it.id.Equals(facebookId))
                .FirstOrDefault();
            if (selectedFacebookAccount == null) return;

            var updateUserProfile = Builders<UserProfile>.Update.Set(it => it.IsFacebookVerified, false);
            var userprofile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName);
            userprofile.UpdateOne(it => it.id.Equals(selectedFacebookAccount.UserId), updateUserProfile);

            facebookTable.DeleteOne(it => it.id.Equals(facebookId));
        }

        /// <summary>
        /// กำหนดทีมที่ชอบ
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนดทีมที่ชอบ</param>
        /// <param name="teamId">รหัสทีมที่ชอบ</param>
        public void SetFavoriteTeam(string userId, string teamId)
        {
            var update = Builders<UserProfile>.Update.Set(it => it.FavouriteTeamId, teamId);
            var userprofile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName);
            userprofile.UpdateOne(it => it.id.Equals(userId), update);
        }

        /// <summary>
        /// กำหนดรหัสสำหรับตรวจสอบเบอร์โทรศัพท์
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="phoneNumber">เบอร์โทรศัพท์ที่ใช้ในการยืนยัน</param>
        /// <param name="verifierCode">รหัสสำหรับตรวจสอบเบอร์โทรศัพท์</param>
        public void SetVerifierPhoneNumber(string userId, string phoneNumber, string verifierCode)
        {
            var update = Builders<UserProfile>.Update
              .Set(it => it.PhoneNo, phoneNumber)
              .Set(it => it.VerifierCode, verifierCode)
              .Set(it => it.VerifiedPhoneDate, null);

            var userprofile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName);
            userprofile.UpdateMany(it => it.id.Equals(userId), update);
        }

        /// <summary>
        /// กำหนดการยืนยันเบอร์โทรศัพท์เสร็จสิ้น
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="completedDate">วันเวลาที่ทำการยืนยันเสร็จสิ้น</param>
        public void SetVerifiedPhoneNumberComplete(string userId, DateTime completedDate)
        {
            var update = Builders<UserProfile>.Update
              .Set(it => it.VerifiedPhoneDate, completedDate);

            var userprofile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName);
            userprofile.UpdateMany(it => it.id.Equals(userId), update);
        }

        /// <summary>
        /// อัพเดทบัญชีผู้ใช้จากการสั่งซื้อคูปอง
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการอัพเดท</param>
        /// <param name="remainingPoints">จำนวนแต้มที่เหลือ</param>
        /// <param name="orderedCoupons">จำนวนคูปองที่สั่งซื้อไปแล้ว</param>
        public void UpdateFromBuyCoupons(string userId, int remainingPoints, int orderedCoupons)
        {
            var update = Builders<UserProfile>.Update
                .Set(it => it.Points, remainingPoints)
                .Set(it => it.OrderedCoupon, orderedCoupons);

            var userprofile = MongoUtil.GetCollection<UserProfile>(UserProfileTableName);
            userprofile.UpdateMany(it => it.id.Equals(userId), update);
        }

        #endregion IAccountRepository members
    }
}
