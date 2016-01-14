using ApiApp.Models;
using ApiApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiApp.Controllers
{
    /// <summary>
    /// Sync API
    /// </summary>
    [RoutePrefix("api/sync")]
    public class SyncController : ApiController
    {
        private IMatchesRepository _matchRepo;
        private IAccountRepository _accountRepo;
        private IPredictionRepository _predictionRepo;
        private IFootballService _svc;

        /// <summary>
        /// Initialize Sync API
        /// </summary>
        /// <param name="matchRepo">Match repository</param>
        /// <param name="accountRepo">Account  repository</param>
        /// <param name="predictionRepo">Prediction  repository</param>
        /// <param name="svc">Football service</param>
        public SyncController(IMatchesRepository matchRepo, IAccountRepository accountRepo, IPredictionRepository predictionRepo, IFootballService svc)
        {
            _matchRepo = matchRepo;
            _accountRepo = accountRepo;
            _predictionRepo = predictionRepo;
            _svc = svc;
        }

        // GET: api/Sync
        /// <summary>
        /// Update all matches
        /// </summary>
        [HttpGet]
        public void Get()
        {
            var now = DateTime.Now;
            var changed = false;
            var allMatches = getAllMatchesFromAPI().ToList();
            var matches = _matchRepo.GetMatchById(allMatches.Select(it => it.match_id)).ToList();
            allMatches.ForEach(match =>
            {
                var apiMatch = convertAPIMatch(match);
                var dbMatch = matches.FirstOrDefault(it => it.id == match.match_id);
                var isMatchChanged = dbMatch == null || apiMatch != dbMatch.ComparableMatch;
                if (isMatchChanged)
                {
                    changed = true;
                    if (dbMatch == null)
                    {
                        dbMatch = new Match
                        {
                            id = match.match_id,
                            LeagueId = match.match_comp_id,
                            Status = match.match_status,
                            TeamAwayId = match.match_visitorteam_id,
                            TeamAwayName = match.match_visitorteam_name,
                            TeamAwayScore = match.match_visitorteam_score,
                            TeamHomeId = match.match_localteam_id,
                            TeamHomeName = match.match_localteam_name,
                            TeamHomeScore = match.match_localteam_score,
                            LeagueName = match.LeagueName,
                            CreatedDateTime = now,
                        };
                    }

                    //dbMatch.BeginDate
                    //dbMatch.BeginUTD
                    //dbMatch.FilterDateTime

                    dbMatch.ComparableMatch = apiMatch;

                    const string PendingStatusCharacter = ":";
                    var shouldUpdateStartedDate = !dbMatch.StartedDate.HasValue && !match.match_status.Contains(PendingStatusCharacter);
                    if (shouldUpdateStartedDate) dbMatch.StartedDate = now;

                    const string CompletedMatchStatus = "FT";
                    var shouldUpdateCompletedMatch = !dbMatch.CompletedDate.HasValue && match.match_status == CompletedMatchStatus;
                    if (shouldUpdateCompletedMatch) dbMatch.CompletedDate = now;

                    dbMatch.GameMinutes = match.match_status;
                    _matchRepo.UpsertMatch(dbMatch);
                }
            });

            if (changed)
            {
                calculateMatches();
                sendNotification();
            }
        }

        // GET: api/Sync/raw
        /// <summary>
        /// Get matches
        /// </summary>
        /// <param name="id">API key</param>
        [HttpGet]
        [Route("raw")]
        public IEnumerable<MatchAPIInformation> Get(string id)
        {
            var result = getAllMatchesFromAPI();
            return result;
        }

        // POST: api/Sync
        /// <summary>
        /// Calculate all matches
        /// </summary>
        [HttpPost]
        public void Post()
        {
            calculateMatches();
        }

        // PUT: api/Sync
        /// <summary>
        /// Send notification to users
        /// </summary>
        [HttpPut]
        public void Put()
        {
            sendNotification();
        }

        private IEnumerable<MatchAPIInformation> getAllMatchesFromAPI()
        {
            var result = _matchRepo.GetAllLeagues().ToList()
                .SelectMany(league =>
                {
                    var now = DateTime.Now.Date.AddDays(league.DifferentDay);
                    const int PreviousOneDay = -1;
                    var fromDate = now.AddDays(PreviousOneDay);
                    const int FutureThreeDays = 3;
                    var toDate = now.AddDays(FutureThreeDays);
                    var matchesResult = _svc.GetMatchesByLeagueId(league.id, fromDate, toDate).ToList();
                    matchesResult.ForEach(it => it.LeagueName = league.Name);
                    return matchesResult;
                });

            return result;
        }

        private string convertAPIMatch(MatchAPIInformation apiMatch)
        {
            // TODO: Not implement
            throw new NotImplementedException();
        }

        private void calculateMatches()
        {
            // TODO: Not implement
            throw new NotImplementedException();
        }

        private void sendNotification()
        {
            var unnotifyMatches = _matchRepo.GetUnNotifyMatches();
            var uniqueDate = unnotifyMatches.Select(it => it.BeginDate.Date).Distinct().ToList();

            // TODO: Notify client with SignalR
        }
    }
}
