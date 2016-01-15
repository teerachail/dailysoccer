using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// แมช์การแข่งขัน
    /// </summary>
    public class Match
    {
        #region Properties

        /// <summary>
        /// รหัสแมช์การแข่งขัน
        /// </summary>
        [BsonId]
        public string id { get; set; }

        /// <summary>
        /// รหัสทีมเจ้าบ้าน
        /// </summary>
        public string TeamHomeId { get; set; }

        /// <summary>
        /// คะแนนทีมเจ้าบ้าน
        /// </summary>
        public int TeamHomeScore { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลทีมเจ้าบ้าน
        /// </summary>
        public int? TeamHomePoint { get; set; }

        /// <summary>
        /// รหัสทีมเยือน
        /// </summary>
        public string TeamAwayId { get; set; }

        /// <summary>
        /// คะแนนทีมเยือน
        /// </summary>
        public int TeamAwayScore { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลทีมเยือน
        /// </summary>
        public int? TeamAwayPoint { get; set; }

        /// <summary>
        /// วันเวลาในการแข่งขัน
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// วันเวลาที่แมทช์การแข่งขันเริ่มแข่งจริง
        /// </summary>
        public DateTime? StartedDate { get; set; }

        /// <summary>
        /// วันเวลาที่แมทช์การแข่งขันจบ
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// วันที่อัพเดทข้อมูลล่าสุด
        /// </summary>
        public DateTime? LastUpdateDateTime { get; set; }

        /// <summary>
        /// วันที่แจ้งเตือนล่าสุด
        /// </summary>
        public DateTime? NotifyDateTime { get; set; }

        /// <summary>
        /// รหัสลีก
        /// </summary>
        public string LeagueId { get; set; }

        /// <summary>
        /// สถานะแมทช์
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// คะแนนที่จะได้เมื่อทายผลเสมอ
        /// </summary>
        public int? DrawPoints { get; set; }

        /// <summary>
        /// ชื่อทีมเจ้าบ้าน
        /// </summary>
        public string TeamHomeName { get; set; }

        /// <summary>
        /// ชื่อทีมเยือน
        /// </summary>
        public string TeamAwayName { get; set; }

        /// <summary>
        /// ชื่อลีก
        /// </summary>
        public string LeagueName { get; set; }

        /// <summary>
        /// วันเวลาแข่ง UDT format
        /// </summary>
        public DateTime? BeginDateTimeUTC { get; set; }

        /// <summary>
        /// วันที่ใช้ในการ filter
        /// </summary>
        public string FilterDate { get; set; }

        /// <summary>
        /// เวลาในเกม
        /// </summary>
        public string GameMinutes { get; set; }

        /// <summary>
        /// วันเวลาที่ทำการคำนวณล่าสุด
        /// </summary>
        public DateTime? LastCalculatedDateTime { get; set; }

        /// <summary>
        /// วันเวลาที่ทำถูก
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// ข้อมูลที่ใช้ตรวจสอบแมช์
        /// </summary>
        public string ComparableMatch { get; set; }

        #endregion Properties
    }
}
