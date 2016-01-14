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
    /// ตัวติดต่อกับการทายผลแมช์การแข่งขัน
    /// </summary>
    public class PredictionRepository : IPredictionRepository
    {
        #region Fields

        private const string PredictionTableName = "dailysoccer.Predictions";

        #endregion Fields

        #region IPredictionRepository members

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
            var predictionId = createPredictionId(userId, matchId);
            var table = MongoUtil.GetCollection<Prediction>(PredictionTableName);
            var isUpdate = table.Find(it => it.id.Equals(predictionId)).Any();
            if (isUpdate)
            {
                var update = Builders<Prediction>.Update
                      .Set(it => it.PredictionTeamId, winnerTeamId)
                      .Set(it => it.PredictionPoints, predictionPoints)
                      .Set(it => it.CreatedDate, currentTime);
                table.UpdateOne(it => it.id.Equals(predictionId), update);
            }
            else
            {
                var newData = new Prediction
                {
                    id = predictionId,
                    PredictionTeamId = winnerTeamId,
                    PredictionPoints = predictionPoints,
                    CreatedDate = currentTime
                };
                table.InsertOne(newData);
            }
        }

        /// <summary>
        /// ยกเลิกการทายผลของผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการกำหนด</param>
        /// <param name="matchId">รหัสแมช์การแข่งขันที่ผู้ใช้ยกเลิก</param>
        public void CancelUserPrediction(string userId, string matchId)
        {
            var deleteId = createPredictionId(userId, matchId);
            var table = MongoUtil.GetCollection<Prediction>(PredictionTableName);
            table.DeleteOne(it => it.id.Equals(deleteId));
        }

        /// <summary>
        /// ดึงประวัติการทายผลของผู้ใช้
        /// </summary>
        public IEnumerable<Prediction> GetUserPredictions()
        {
            var qry = MongoUtil.GetCollection<Prediction>(PredictionTableName)
                .Find(it => true)
                .ToEnumerable();
            return qry;
        }

        private static string createPredictionId(string userId, string matchId)
        {
            return string.Format("{0}-{1}", userId, matchId);
        }

        /// <summary>
        /// อัพเดทข้อมูลการทาย
        /// </summary>
        /// <param name="prediction">ข้อมูลการทายจะดำเนินการ</param>
        public void UpdatePrediction(Prediction prediction)
        {
            var update = Builders<Prediction>.Update
              .Set(it => it.CompletedDate, prediction.CompletedDate)
              .Set(it => it.PredictionPoints, prediction.PredictionPoints)
              .Set(it => it.ActualPoints, prediction.ActualPoints)
              .Set(it => it.CreatedDate, prediction.CreatedDate)
              .Set(it => it.PredictionPoints, prediction.PredictionPoints)
              .Set(it => it.PredictionTeamId, prediction.PredictionTeamId);
              MongoUtil.GetCollection<Prediction>(PredictionTableName)
              .UpdateOne(it => it.id == prediction.id, update);
        }

        #endregion IMatchesRepository members
    }
}
