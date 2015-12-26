﻿using ApiApp.Models;
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
        /// ดึงรายการของรางวัลล่าสุด
        /// </summary>
        IEnumerable<Reward> GetCurrentRewards();

        /// <summary>
        /// ดึงรายการของรางวัลและผู้ชนะล่าสุด
        /// </summary>
        IEnumerable<RewardWinner> GetCurrentWinners();

        /// <summary>
        /// ดึงของรางวัลที่ผู้ใช้เคยได้
        /// </summary>
        /// <param name="userId">รหัสบัญชีผู้ใช้ที่ต้องการดึงข้อมูล</param>
        IEnumerable<MyReward> GetUserRewards(string userId);

        #endregion Methods
    }
}