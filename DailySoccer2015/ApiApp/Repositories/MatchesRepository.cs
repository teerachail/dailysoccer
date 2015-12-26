﻿using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับแมช์การแข่งขัน
    /// </summary>
    public class MatchesRepository : IMatchesRepository
    {
        #region IMatchesRepository members

        /// <summary>
        /// ดึงแมช์การแข่งขันจากวันที่
        /// </summary>
        /// <param name="date">วันที่ที่ต้องการดึงข้อมูล</param>
        public IEnumerable<MatchInformation> GetMatchesByDate(DateTime date)
        {
            // TODO: GetMatchesByDate
            throw new NotImplementedException();
        }

        /// <summary>
        /// ดึงลีกทั้งหมดในระบบ
        /// </summary>
        public IEnumerable<League> GetAllLeagues()
        {
            // TODO: GetAllLeagues
            throw new NotImplementedException();
        }

        /// <summary>
        /// ดึงทีมจากรหัสลีก
        /// </summary>
        /// <param name="leagueId">รหัสลีกที่ต้องการดึงข้อมูล</param>
        public IEnumerable<Team> GetTeamsByLeagueId(string leagueId)
        {
            // TODO: GetTeamsByLeagueId
            throw new NotImplementedException();
        }

        #endregion IMatchesRepository members
    }
}