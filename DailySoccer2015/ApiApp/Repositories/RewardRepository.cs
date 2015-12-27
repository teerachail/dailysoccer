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
        /// ดึงรายการกลุ่มของรางวัล
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RewardGroup> GetRewardGroups()
        {
            return MongoAccess.MongoUtil.GetRewardGroups();
        }

        /// <summary>
        /// ดึงรายการของรางวัล
        /// </summary>
        public IEnumerable<Reward> GetRewards()
        {
            return MongoAccess.MongoUtil.GetRewards();
        }

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะ
        /// </summary>
        public IEnumerable<Winner> GetWinners()
        {
            return MongoAccess.MongoUtil.GetWinners();
        }

        #endregion IRewardRepository members
    }
}
