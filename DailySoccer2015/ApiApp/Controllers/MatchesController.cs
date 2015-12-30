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
        /// <param name="month">Filter by month</param>
        /// <param name="year">Filter by year</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{day}/{month}/{year}")]
        public IEnumerable<MatchInformation> Get(int day, int month, int year)
        {
            var matches = _repo.GetMatches();
            var teams = _repo.GetTeams();
            var leagues = _repo.GetAllLeagues();

            var selectedDate = new DateTime(year, month, day);
            
            var selectedMatch = from match in matches.Where(it => it.BeginDate.Date == selectedDate)
                                let teamHomeName = teams.First(team => team.id == match.TeamHomeId).Name
                                let teamAwayName = teams.First(team => team.id == match.TeamAwayId).Name
                                let leagueName = leagues.First(league => league.id == match.LeagueId).Name
                                select new MatchInformation
                                {
                                     id = match.id,
                                     TeamHomeId = match.TeamHomeId,
                                     TeamHomeName = teamHomeName,
                                     TeamHomePoint = match.TeamHomePoint,
                                     TeamHomeScore = match.TeamHomeScore,
                                     TeamAwayId = match.TeamAwayId,
                                     TeamAwayName = teamAwayName,
                                     TeamAwayPoint = match.TeamAwayPoint,
                                     TeamAwayScore = match.TeamAwayScore,
                                     DrawPoints = match.DrawPoints,
                                     BeginDate = match.BeginDate,
                                     Status = match.Status,
                                     StartedDate = match.StartedDate,
                                     CompletedDate = match.CompletedDate,
                                     LeagueId = match.LeagueId,
                                     LeagueName = leagueName
                                };

            return selectedMatch;
        }
    }
}
