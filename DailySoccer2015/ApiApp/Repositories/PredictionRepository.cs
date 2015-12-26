using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับการทายผลแมช์การแข่งขัน
    /// </summary>
    public class PredictionRepository : IPredictionRepository
    {
        #region IPredictionRepository members

        /// <summary>
        /// ดึงการทายผลของผู้ใช้จากวันที่
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการดึง</param>
        /// <param name="date">วันที่ที่ต้องการดึงข้อมูล</param>
        public IEnumerable<PredictionInformation> GetUserPredictionsByDate(string userId, DateTime date)
        {
            // TODO: GetUserPredictionsByDate
            throw new NotImplementedException();
        }

        /// <summary>
        /// กำหนดการทายผลของผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการกำหนด</param>
        /// <param name="matchId">รหัสแมช์การแข่งขันที่ผู้ใช้เลือก</param>
        /// <param name="winnerTeamId">รหัสทีมที่ผู้ใช้เลือก</param>
        /// <param name="predictionPoints">คะแนนที่ผู้ใช้จะได้ถ้าทายถูก</param>
        /// <param name="currentTime">วันเวลาที่ทำการบันทึกข้อมูล</param>
        public void SetUserPrediction(string userId, string matchId, string winnerTeamId, int predictionPoints, DateTime currentTime)
        {
            // TODO: SetUserPrediction
            throw new NotImplementedException();
        }

        /// <summary>
        /// ยกเลิกการทายผลของผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการกำหนด</param>
        /// <param name="matchId">รหัสแมช์การแข่งขันที่ผู้ใช้ยกเลิก</param>
        public void CancelUserPrediction(string userId, string matchId)
        {
            // TODO: CancelUserPrediction
            throw new NotImplementedException();
        }

        #endregion IMatchesRepository members
    }
}
