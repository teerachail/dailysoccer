using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับข้อมูลของรางวัล
    /// </summary>
    public interface IRewardRepository
    {
        #region Methods

        /// <summary>
        /// ดึงรายการกลุ่มของรางวัล
        /// </summary>
        /// <returns></returns>
        IEnumerable<RewardGroup> GetRewardGroups();

        /// <summary>
        /// ดึงรายการของรางวัล
        /// </summary>
        IEnumerable<Reward> GetRewards();

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะ
        /// </summary>
        IEnumerable<Winner> GetWinners();

        #endregion Methods
    }
}
