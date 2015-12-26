﻿using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับบัญชีผู้ใช้
    /// </summary>
    public interface IAccountRepository
    {
        #region Methods

        /// <summary>
        /// สร้างบัญชีผู้ใช้ใหม่
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการสร้าง</param>
        void CreateUserProfile(string userId);

        /// <summary>
        /// ดึงบัญชีผู้ใช้จากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ใช้ในการตรวจสอบ</param>
        UserProfile GetUserProfileById(string userId);

        /// <summary>
        /// ดึงบัญชีผู้ใช้จากรหัส Facebook
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ใช้ในการตรวจสอบ</param>
        UserProfile GetUserProfileByFacebookId(string facebookId);

        /// <summary>
        /// ผูกบัญชี Facebook เข้ากับบัญชีผู้ใช้
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการจะผูก</param>
        /// <param name="userId">บัญชีผู้ใช้ที่จะทำการผูก</param>
        void TieFacebookAccount(string facebookId, string userId);

        /// <summary>
        /// ยกเลิกการผูกบัญชี Facebook
        /// </summary>
        /// <param name="facebookId">รหัส Facebook ที่ต้องการยกเลิกการผูก</param>
        void UntieFacebookAccount(string facebookId);

        /// <summary>
        /// กำหนดทีมที่ชอบ
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนดทีมที่ชอบ</param>
        /// <param name="teamId">รหัสทีมที่ชอบ</param>
        void SetFavoriteTeam(string userId, int teamId);

        /// <summary>
        /// กำหนดรหัสสำหรับตรวจสอบเบอร์โทรศัพท์
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="phoneNumber">เบอร์โทรศัพท์ที่ใช้ในการยืนยัน</param>
        /// <param name="verifierCode">รหัสสำหรับตรวจสอบเบอร์โทรศัพท์</param>
        void SetVerifierPhoneNumber(string userId, string phoneNumber, string verifierCode);

        /// <summary>
        /// กำหนดการยืนยันเบอร์โทรศัพท์เสร็จสิ้น
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่จะทำการกำหนด</param>
        /// <param name="completedDate">วันเวลาที่ทำการยืนยันเสร็จสิ้น</param>
        void SetVerifiedPhoneNumberComplete(string userId, DateTime completedDate);

        #endregion Methods
    }
}