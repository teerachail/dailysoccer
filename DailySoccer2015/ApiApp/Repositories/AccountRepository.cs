using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    ///ตัวติดต่อกับบัญชีผู้ใช้
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        #region IAccountRepository members

        /// <summary>
        /// สร้างบัญชีผู้ใช้ใหม่
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการสร้าง</param>
        public void CreateUserProfile(string userId)
        {
            // TODO: CreateUserProfile
            throw new NotImplementedException();
        }

        /// <summary>
        /// ดึงบัญชีผู้ใช้จากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ใช้ในการตรวจสอบ</param>
        public UserProfile GetUserProfileById(string userId)
        {
            // TODO: GetUserProfileById
            throw new NotImplementedException();
        }

        /// <summary>
        /// ดึงบัญชีผู้ใช้จากรหัส Facebook
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ใช้ในการตรวจสอบ</param>
        public UserProfile GetUserProfileByFacebookId(string facebookId)
        {
            // TODO: GetUserProfileByFacebookId
            throw new NotImplementedException();
        }

        /// <summary>
        /// ผูกบัญชี Facebook เข้ากับบัญชีผู้ใช้
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการจะผูก</param>
        /// <param name="userId">บัญชีผู้ใช้ที่จะทำการผูก</param>
        public void TieFacebookAccount(string facebookId, string userId)
        {
            // TODO: TieFacebookAccount
            throw new NotImplementedException();
        }

        /// <summary>
        /// ยกเลิกการผูกบัญชี Facebook
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการยกเลิกการผูก</param>
        public void UntieFacebookAccount(string facebookId)
        {
            // TODO: UntieFacebookAccount
            throw new NotImplementedException();
        }

        /// <summary>
        /// กำหนดทีมที่ชอบ
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนดทีมที่ชอบ</param>
        /// <param name="teamId">รหัสทีมที่ชอบ</param>
        public void SetFavoriteTeam(string userId, int teamId)
        {
            // TODO: SetFavoriteTeam
            throw new NotImplementedException();
        }

        /// <summary>
        /// กำหนดรหัสสำหรับตรวจสอบเบอร์โทรศัพท์
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="phoneNumber">เบอร์โทรศัพท์ที่ใช้ในการยืนยัน</param>
        /// <param name="verifierCode">รหัสสำหรับตรวจสอบเบอร์โทรศัพท์</param>
        public void SetVerifierPhoneNumber(string userId, string phoneNumber, string verifierCode)
        {
            // TODO: SetVerifierPhoneNumber
            throw new NotImplementedException();
        }

        /// <summary>
        /// กำหนดการยืนยันเบอร์โทรศัพท์เสร็จสิ้น
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="completedDate">วันเวลาที่ทำการยืนยันเสร็จสิ้น</param>
        public void SetVerifiedPhoneNumberComplete(string userId, DateTime completedDate)
        {
            // TODO: SetVerifiedPhoneNumberComplete
            throw new NotImplementedException();
        }

        #endregion IAccountRepository members
    }
}
