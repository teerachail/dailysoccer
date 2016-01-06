﻿using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับแมช์การแข่งขัน
    /// </summary>
    public interface IMatchesRepository
    {
        #region Methods

        /// <summary>
        /// ดึงแมช์การแข่งขันทั้งหมดในระบบ
        /// </summary>
        IEnumerable<Match> GetAllMatches();

        /// <summary>
        /// ดึงแมช์การแข่งขันจากรหัสการแข่งขัน
        /// </summary>
        /// <param name="matchId">รหัสการแข่งขันที่ต้องการดึงข้อมูล</param>
        Match GetMatchById(string matchId);

        /// <summary>
        /// ดึงแมช์การแข่งขันจากปี
        /// </summary>
        /// <param name="year">ปีที่ต้องการดึงข้อมูล</param>
        IEnumerable<Match> GetMatchesByYear(int year);

        /// <summary>
        /// ดึงลีกทั้งหมดในระบบ
        /// </summary>
        IEnumerable<League> GetAllLeagues();

        /// <summary>
        /// ดึงรายการลีกจากรหัสลีก
        /// </summary>
        /// <param name="leagueIds">รหัสลีกที่ต้องการดึง</param>
        IEnumerable<League> GetLeaguesByIds(IEnumerable<string> leagueIds);

        /// <summary>
        /// ดึงรายการทีมจากรหัสลีก
        /// </summary>
        /// <param name="leagueId">รหัสลีกที่ต้องการดึงข้อมูล</param>
        IEnumerable<Team> GetTeamsByLeagueId(string leagueId);

        /// <summary>
        /// ดึงทีมจากรหัสทีม
        /// </summary>
        Team GetTeamById(string teamId);

        #endregion Methods
    }
}
