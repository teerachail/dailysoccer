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
        /// ดึงรายการกลุ่มของรางวัลล่าสุด
        /// </summary>
        RewardGroup GetCurrentRewardGroups();

        /// <summary>
        /// ดึงรายการของรางวัลจากรหัสกลุ่มของรางวัล
        /// </summary>
        /// <param name="rewardGroupId">รหัสกลุ่มของรางวัลที่ต้องการหา</param>
        IEnumerable<Reward> GetRewardsByRewardGroupId(string rewardGroupId);

        /// <summary>
        /// ดึงรายการของรางวัลจากรหัสของรางวัล
        /// </summary>
        /// <param name="rewardIds">รายการรหัสของรางวัลที่ต้องการหา</param>
        IEnumerable<Reward> GetRewardsByIds(IEnumerable<string> rewardIds);

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้</param>
        IEnumerable<Winner> GetWinnersByUserId(string userId);

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="rewardId">รหัสบัญชีผู้ใช้</param>
        IEnumerable<Winner> GetWinnersByRewardId(string rewardId);

        #endregion Methods
    }
}
