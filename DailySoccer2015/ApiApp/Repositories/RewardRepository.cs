using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ApiApp.MongoAccess;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับข้อมูลของรางวัล
    /// </summary>
    public class RewardRepository : IRewardRepository
    {
        #region Fields

        private const string RewardGroupTableName = "dailysoccer.RewardGroups";
        private const string RewardTableName = "dailysoccer.Rewards";
        private const string WinnerTableName = "dailysoccer.Winners";

        #endregion Fields

        #region IRewardRepository members

        /// <summary>
        /// ดึงรายการกลุ่มของรางวัลล่าสุด
        /// </summary>
        public RewardGroup GetCurrentRewardGroup()
        {
            var currentRewardGroup = MongoUtil.GetCollection<RewardGroup>(RewardGroupTableName)
                .Find(it => true)
                .SortByDescending(it => it.ExpiredDate)
                .FirstOrDefault();
            return currentRewardGroup;
        }

        /// <summary>
        /// ดึงรายการกลุ่มของรางวัลล่าที่จบแล้วล่าสุด
        /// </summary>
        public RewardGroup GetLastCompletedRewardGroup()
        {
            const int SkipTheCurrentRewardGroup = 1;
            var currentRewardGroup = MongoUtil.GetCollection<RewardGroup>(RewardGroupTableName)
                .Find(it => true)
                .SortByDescending(it => it.ExpiredDate)
                .Skip(SkipTheCurrentRewardGroup)
                .FirstOrDefault();
            return currentRewardGroup;
        }

        /// <summary>
        /// ดึงรายการของรางวัลจากรหัสกลุ่มของรางวัล
        /// </summary>
        /// <param name="rewardGroupId">รหัสกลุ่มของรางวัลที่ต้องการหา</param>
        public IEnumerable<Reward> GetRewardsByRewardGroupId(string rewardGroupId)
        {
            var qry = MongoUtil.GetCollection<Reward>(RewardTableName)
                .Find(it => it.RewardGroupId.Equals(rewardGroupId))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงรายการของรางวัลจากรหัสของรางวัล
        /// </summary>
        /// <param name="rewardIds">รายการรหัสของรางวัลที่ต้องการหา</param>
        public IEnumerable<Reward> GetRewardsByIds(IEnumerable<string> rewardIds)
        {
            var qry = MongoUtil.GetCollection<Reward>(RewardTableName)
                .Find(it => rewardIds.Contains(it.id))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงรายการของผู้โชคดีจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้</param>
        public IEnumerable<Winner> GetWinnersByUserId(string userId)
        {
            var qry = MongoUtil.GetCollection<Winner>(WinnerTableName)
               .Find(it => it.UserId.Equals(userId))
               .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ดึงรายการของผู้โชคดีจากรหัสของรางวัล
        /// </summary>
        /// <param name="rewardIds">รหัสของรางวัลที่ต้องการขอ</param>
        public IEnumerable<Winner> GetWinnersByRewardIds(IEnumerable<string> rewardIds)
        {
            var qry = MongoUtil.GetCollection<Winner>(WinnerTableName)
                .Find(it => rewardIds.Contains(it.RewardId))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// สร้างรายการผู้โชคดีใหม่
        /// </summary>
        /// <param name="winners">ข้อมูลรายการผู้โชคดี</param>
        public void CreateNewWinners(IEnumerable<Winner> winners)
        {
            MongoUtil.GetCollection<Winner>(WinnerTableName).InsertMany(winners);
        }

        #endregion IRewardRepository members
    }
}
