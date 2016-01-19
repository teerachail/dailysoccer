using ApiApp.Models;
using ApiApp.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiApp.Controllers
{
    /// <summary>
    /// Matches API
    /// </summary>
    [RoutePrefix("api/matches")]
    public class MatchesController : ApiController
    {
        private IMatchesRepository _repo;

        /// <summary>
        /// Initialize Leagues API
        /// </summary>
        /// <param name="repo">Matches repository</param>
        public MatchesController(IMatchesRepository repo)
        {
            _repo = repo;
        }

        // GET: api/matches/30/12/2015
        /// <summary>
        /// Get match by date
        /// </summary>
        /// <param name="day">Filter by day</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{day}")]
        public IEnumerable<LeagueInformation> Get(int day)
        {
            const int DayRange = 3;
            var fromDate = DateTime.Now.AddDays(-DayRange);
            var toDate = DateTime.Now.AddDays(DayRange);
            var dateRange = Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1).Select(d => fromDate.AddDays(d));
            var selectedDate = dateRange.FirstOrDefault(it => it.Date.Day == day);
            if (selectedDate == null) return null;

            var matches = _repo.GetMatchesByDate(selectedDate).ToList();
            var result = matches
                .Where(it => it.BeginDateTimeUTC.HasValue)
                .Where(it => it.FilterDate == MatchesRepository.ConvertDateTimeToFilterDateFormat(selectedDate))
                .Select(match => new MatchInformation
                {
                    id = match.id,
                    TeamHomeId = match.TeamHomeId,
                    TeamHomeName = match.TeamHomeName,
                    TeamHomePoint = match.TeamHomePoint,
                    TeamHomeScore = match.TeamHomeScore,
                    TeamAwayId = match.TeamAwayId,
                    TeamAwayName = match.TeamAwayName,
                    TeamAwayPoint = match.TeamAwayPoint,
                    TeamAwayScore = match.TeamAwayScore,
                    DrawPoints = match.DrawPoints,
                    BeginDate = match.BeginDateTimeUTC.Value,
                    Status = match.Status,
                    StartedDate = match.StartedDate,
                    CompletedDate = match.CompletedDate,
                    LeagueId = match.LeagueId,
                    LeagueName = match.LeagueName,
                })
                .GroupBy(it => it.LeagueName)
                .Select(it => new LeagueInformation
                {
                    Name = it.Key,
                    Matches = it.OrderBy(match => match.BeginDate).ToList()
                }).ToList();

            return result;
        }
    }
}
