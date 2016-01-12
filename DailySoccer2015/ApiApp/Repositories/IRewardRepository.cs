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
        RewardGroup GetCurrentRewardGroup();

        /// <summary>
        /// ดึงรายการกลุ่มของรางวัลล่าที่จบแล้วล่าสุด
        /// </summary>
        RewardGroup GetLastCompletedRewardGroup();

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
        /// ดึงรายการของผู้โชคดีจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้</param>
        IEnumerable<Winner> GetWinnersByUserId(string userId);

        /// <summary>
        /// ดึงรายการของผู้โชคดีจากรหัสของรางวัล
        /// </summary>
        /// <param name="rewardIds">รหัสของรางวัลที่ต้องการขอ</param>
        IEnumerable<Winner> GetWinnersByRewardIds(IEnumerable<string> rewardIds);

        #endregion Methods
    }
}
