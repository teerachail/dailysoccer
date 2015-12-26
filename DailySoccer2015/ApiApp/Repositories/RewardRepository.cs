using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับข้อมูลของรางวัล
    /// </summary>
    public class RewardRepository : IRewardRepository
    {
        #region IRewardRepository members

        /// <summary>
        /// ดึงรายการของรางวัลล่าสุด
        /// </summary>
        public IEnumerable<Reward> GetCurrentRewards()
        {
            // TODO: GetCurrentRewards
            throw new NotImplementedException();
        }

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะล่าสุด
        /// </summary>
        public IEnumerable<RewardWinner> GetCurrentWinners()
        {
            // TODO: GetCurrentWinners
            throw new NotImplementedException();
        }

        /// <summary>
        /// ดึงของรางวัลที่ผู้ใช้เคยได้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการดึงข้อมูล</param>
        public IEnumerable<MyReward> GetUserRewards(string userId)
        {
            // TODO: GetUserRewards
            throw new NotImplementedException();
        }

        #endregion IRewardRepository members
    }
}
