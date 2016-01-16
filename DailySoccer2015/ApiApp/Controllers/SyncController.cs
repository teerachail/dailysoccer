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
        private enum GameResult { TeamHomeWin, TeamAwayWin, GameDraw }
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
                        int teamAwayScore;
                        int.TryParse(match.match_visitorteam_score, out teamAwayScore);

                        int teamHomeScore;
                        int.TryParse(match.match_localteam_score, out teamHomeScore);

                        dbMatch = new Match
                        {
                            id = match.match_id,
                            LeagueId = match.match_comp_id,
                            Status = match.match_status,
                            TeamAwayId = match.match_visitorteam_id,
                            TeamAwayName = match.match_visitorteam_name,
                            TeamAwayScore = teamAwayScore,
                            TeamHomeId = match.match_localteam_id,
                            TeamHomeName = match.match_localteam_name,
                            TeamHomeScore = teamHomeScore,
                            LeagueName = match.LeagueName,
                            CreatedDateTime = now,
                        };
                    }

                    const string PendingStatusCharacter = ":";
                    var shouldUpdateStartedDate = !dbMatch.StartedDate.HasValue && !match.match_status.Contains(PendingStatusCharacter);
                    if (shouldUpdateStartedDate) dbMatch.StartedDate = now;

                    const string CompletedMatchStatus = "FT";
                    var shouldUpdateCompletedMatch = !dbMatch.CompletedDate.HasValue && match.match_status == CompletedMatchStatus;
                    if (shouldUpdateCompletedMatch) dbMatch.CompletedDate = now;

                    const string DateFormat = "dd.MM.yyyy";
                    var provider = System.Globalization.CultureInfo.GetCultureInfo(match.TimeZone);
                    var matchDate = DateTime.ParseExact(match.match_formatted_date, DateFormat, provider);
                    var matchTime = string.IsNullOrWhiteSpace(match.match_time) ? TimeSpan.Zero : TimeSpan.Parse(match.match_time);

                    dbMatch.FilterDate = matchDate.AddDays(match.DifferentDay).ToString("yyyyMMdd");
                    dbMatch.BeginDateTimeUTC = matchDate.Add(matchTime).ToUniversalTime();

                    dbMatch.LastUpdateDateTime = now;
                    dbMatch.ComparableMatch = apiMatch;
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
            var result = _matchRepo.GetAllLeagues()
                .ToList()
                .SelectMany(league =>
                {
                    var now = DateTime.Now.Date.AddDays(league.DifferentDay);
                    const int PreviousOneDay = -1;
                    var fromDate = now.AddDays(PreviousOneDay);
                    const int FutureThreeDays = 3;
                    var toDate = now.AddDays(FutureThreeDays);
                    var matchesResult = _svc.GetMatchesByLeagueId(league.id, fromDate, toDate).ToList();
                    matchesResult.ForEach(it =>
                    {
                        it.LeagueName = league.Name;
                        it.DifferentDay = league.DifferentDay;
                        // TODO: Match's TimeZone
                        it.TimeZone = "en-GB";
                    });
                    return matchesResult;
                });

            return result;
        }

        private string convertAPIMatch(MatchAPIInformation apiMatch)
        {
            return apiMatch.match_status;
        }

        private void calculateMatches()
        {
            var now = DateTime.Now;
            var matches = _matchRepo.GetAllMatches().ToList();
            var predictions = _predictionRepo.GetUserPredictions().ToList();
            var predictionPoints = new[] { 100, 120, 130, 140, 150 };

            var changedMatches = matches.Where(it => !it.LastCalculatedDateTime.HasValue || it.LastUpdateDateTime > it.LastCalculatedDateTime).ToList();
            changedMatches.ForEach(match =>
            {
                var prediction = predictions.Where(predict => predict.PredictionTeamId == match.id).ToList();
                GameResult gameResult;
                GameResult userPrediction;

                if (match.TeamHomeScore > match.TeamAwayScore) gameResult = GameResult.TeamHomeWin;
                else if (match.TeamHomeScore < match.TeamAwayScore) gameResult = GameResult.TeamAwayWin;
                else gameResult = GameResult.GameDraw;

                prediction.ForEach(predict =>
                {
                    if (predict.PredictionTeamId == match.TeamHomeId) userPrediction = GameResult.TeamHomeWin;
                    else if (predict.PredictionTeamId == match.TeamAwayId) userPrediction = GameResult.TeamAwayWin;
                    else userPrediction = GameResult.GameDraw;

                    if (gameResult == userPrediction) predict.ActualPoints = predict.PredictionPoints;
                    else predict.ActualPoints = 0;
                    _predictionRepo.UpdatePrediction(predict);

                    const char splitSeparetor = '-';
                    const int userIdPosition = 0;
                    var users = _accountRepo.GetUserProfileById(predict.id.Split(splitSeparetor)[userIdPosition]);
                    users.Points += predict.PredictionPoints;
                    _accountRepo.UpdatePoint(users.id, users.Points);
                });

                var random = new Random();
                var teamHomePoint = predictionPoints[random.Next(predictionPoints.Length)];
                var teamAwayPoint = predictionPoints[random.Next(predictionPoints.Length)];
                match.TeamHomePoint = teamHomePoint;
                match.TeamAwayPoint = teamAwayPoint;
                match.LastCalculatedDateTime = now;
                _matchRepo.UpsertMatch(match);
            });
        }

        private void sendNotification()
        {
            var unnotifyMatches = _matchRepo.GetUnNotifyMatches();
            var uniqueDate = unnotifyMatches.Select(it => it.BeginDate.Date).Distinct().ToList();

            // TODO: Notify client with SignalR
        }
    }
}
