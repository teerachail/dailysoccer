using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// บัญชีผู้ใช้
    /// </summary>
    public class UserProfile
    {
        #region Properties

        /// <summary>
        /// รหัสผู้ใช้
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// คะแนนที่มี
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// จำนวนคูปองที่ซื้อไว้
        /// </summary>
        public int OrderedCoupon { get; set; }

        /// <summary>
        /// เบอร์โทรศัพท์
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// รหัสสำหรับตรวจสอบเบอร์โทรศัพท์
        /// </summary>
        public string VerifierCode { get; set; }

        /// <summary>
        /// วันเวลาที่ทำการยืนยันเบอร์โทรศัพท์เสร็จสิ้น
        /// </summary>
        public DateTime? VerifiedPhoneDate { get; set; }

        /// <summary>
        /// สถานะการผูกบัญชีเข้ากับ Facebook เสร็จสิ้น
        /// </summary>
        public bool IsFacebookVerified { get; set; }

        /// <summary>
        /// รหัสทีมที่ชอบ
        /// </summary>
        public string FavouriteTeamId { get; set; }

        #endregion Properties
    }
}
