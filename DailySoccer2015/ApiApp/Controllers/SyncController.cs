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
                    if (dbMatch == null) dbMatch = new Match { id = match.match_id, CreatedDateTime = now, };

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

                    dbMatch.FilterDate = MatchesRepository.ConvertDateTimeToFilterDateFormat(matchDate.AddDays(match.DifferentDay));
                    dbMatch.BeginDateTimeUTC = matchDate.Add(matchTime).ToUniversalTime();

                    int teamAwayScore;
                    int.TryParse(match.match_visitorteam_score, out teamAwayScore);
                    dbMatch.TeamAwayScore = teamAwayScore;

                    int teamHomeScore;
                    int.TryParse(match.match_localteam_score, out teamHomeScore);
                    dbMatch.TeamHomeScore = teamHomeScore;

                    dbMatch.LeagueId = match.match_comp_id;
                    dbMatch.TeamAwayId = match.match_visitorteam_id;
                    dbMatch.TeamAwayName = match.match_visitorteam_name;
                    dbMatch.TeamHomeId = match.match_localteam_id;
                    dbMatch.TeamHomeName = match.match_localteam_name;
                    dbMatch.LeagueName = match.LeagueName;
                    dbMatch.LastUpdateDateTime = now;
                    dbMatch.ComparableMatch = apiMatch;
                    dbMatch.GameMinutes = match.match_status;
                    dbMatch.Status = match.match_status;

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

        // PUT: api/Sync/5
        [HttpPut]
        [Route("internalupdate")]
        public string Internalupdate()
        {
            var now = DateTime.Now;
            var mongoDb = MongoAccess.MongoUtil.GetCollection<MatchTimeLine>("dailysoccer.MatchTimeLine");
            var changedMatches = mongoDb.Find(it => now > it.UpdateTime && !it.IsAlreadyUsed).ToEnumerable().OrderBy(it => it.UpdateTime).ToList();
            var dbMatches = _matchRepo.GetMatchById(changedMatches.Select(it => it.MatchId)).ToList();

            foreach (var item in changedMatches)
            {
                var selectedMatch = dbMatches.FirstOrDefault(it => it.id == item.MatchId);
                if (selectedMatch == null) continue;

                selectedMatch.StartedDate = item.StartedDate;
                selectedMatch.CompletedDate = item.CompletedDate;
                selectedMatch.TeamAwayScore = item.TeamAwayScore;
                selectedMatch.TeamHomeScore = item.TeamHomeScore;
                selectedMatch.LastUpdateDateTime = now;
                selectedMatch.ComparableMatch = item.Status;
                selectedMatch.GameMinutes = item.Status;
                selectedMatch.Status = item.Status;
                _matchRepo.UpsertMatch(selectedMatch);
            }
            calculateMatches();

            foreach (var item in changedMatches)
            {
                var update = Builders<MatchTimeLine>.Update.Set(it => it.IsAlreadyUsed, true);
                mongoDb.UpdateOne(it => it.id == item.id, update);
            }

            return now.ToString();
        }
        public class MatchTimeLine
        {
            [MongoDB.Bson.Serialization.Attributes.BsonId]
            public string id { get; set; }
            public DateTime UpdateTime { get; set; }
            public string Status { get; set; }
            public int TeamAwayScore { get; set; }
            public int TeamHomeScore { get; set; }
            public string MatchId { get; set; }
            public DateTime StartedDate { get; set; }
            public DateTime? CompletedDate { get; set; }
            public bool IsAlreadyUsed { get; set; }
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

            var changedMatches = matches.Where(it => !it.LastCalculatedDateTime.HasValue 
            || (it.LastUpdateDateTime.HasValue && it.LastUpdateDateTime > it.LastCalculatedDateTime))
            .ToList();
            changedMatches.ForEach(match =>
            {
                if (match.CompletedDate.HasValue)
                {
                    GameResult gameResult;
                    if (match.TeamHomeScore > match.TeamAwayScore) gameResult = GameResult.TeamHomeWin;
                    else if (match.TeamHomeScore < match.TeamAwayScore) gameResult = GameResult.TeamAwayWin;
                    else gameResult = GameResult.GameDraw;

                    //var prediction = predictions.Where(predict => predict.PredictionTeamId == match.id).ToList();
                    var prediction = predictions.Where(it =>
                    {
                        var matchId = it.id.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                        if (string.IsNullOrEmpty(matchId)) return false;
                        return matchId == match.id;
                    }).ToList();

                    prediction.ForEach(predict =>
                    {
                        GameResult userPrediction;
                        if (predict.PredictionTeamId == match.TeamHomeId) userPrediction = GameResult.TeamHomeWin;
                        else if (predict.PredictionTeamId == match.TeamAwayId) userPrediction = GameResult.TeamAwayWin;
                        else userPrediction = GameResult.GameDraw;

                        if (gameResult == userPrediction) predict.ActualPoints = predict.PredictionPoints;
                        else predict.ActualPoints = 0;
                        predict.CompletedDate = now;
                        _predictionRepo.UpdatePrediction(predict);

                        const char splitSeparetor = '-';
                        const int userIdPosition = 0;
                        var users = _accountRepo.GetUserProfileById(predict.id.Split(splitSeparetor)[userIdPosition]);
                        users.Points += predict.ActualPoints;
                        _accountRepo.UpdatePoint(users.id, users.Points);
                    });
                }

                var random = new Random();
                var teamHomePoint = predictionPoints[random.Next(predictionPoints.Length)];
                var teamAwayPoint = predictionPoints[random.Next(predictionPoints.Length)];
                var drawPoint = predictionPoints[random.Next(predictionPoints.Length)];
                match.TeamHomePoint = teamHomePoint;
                match.TeamAwayPoint = teamAwayPoint;
                match.DrawPoints = drawPoint;
                match.LastCalculatedDateTime = now;
                _matchRepo.UpsertMatch(match);
            });
        }

        private void sendNotification()
        {
            var unnotifyMatches = _matchRepo.GetUnNotifyMatches();
            var uniqueDate = unnotifyMatches
                .Where(it => it.FilterDate != null)
                .Select(it => it.FilterDate)
                .Distinct()
                .ToList();

            // TODO: Notify client with SignalR
        }
    }
}
