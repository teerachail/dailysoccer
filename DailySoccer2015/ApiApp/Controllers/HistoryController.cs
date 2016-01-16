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
    /// History API
    /// </summary>
    [RoutePrefix("api/history")]
    public class HistoryController : ApiController
    {
        private IMatchesRepository _matchesRepo;
        private IPredictionRepository _predictionRepo;

        /// <summary>
        /// Initialize History API
        /// </summary>
        /// <param name="matchesRepo">Matches repository</param>
        /// <param name="predictionRepo">Prediction repository</param>
        public HistoryController(IMatchesRepository matchesRepo, IPredictionRepository predictionRepo)
        {
            _matchesRepo = matchesRepo;
            _predictionRepo = predictionRepo;
        }

        // GET: api/History/5
        /// <summary>
        /// Get summary user predictions
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet]
        public IEnumerable<PredictionMonthlySummary> Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            const int IgnoreMonth = 0;
            var now = DateTime.Now;
            var matches = _matchesRepo.GetMatchesByYear(now.Year).Where(it => it.FilterDateMonth != IgnoreMonth).ToList();
            var predictionQry = getPredictions(id, matches);

            var result = new List<PredictionMonthlySummary>();
            var months = matches.Select(it => it.FilterDateMonth).Distinct().OrderByDescending(it => it);
            foreach (var month in months)
            {
                var todayMatches = matches.Where(it => it.FilterDateMonth == month);
                var totalPoints = getPredictions(todayMatches, predictionQry).Sum(it => it.ActualPoints);

                var date = new DateTime(now.Year, month, 1);
                result.Add(new PredictionMonthlySummary { Date = date, TotalPoints = totalPoints });
            }
            return result.OrderByDescending(it => it.Date);
        }

        // GET: api/History/5/2015/12
        /// <summary>
        /// Get daily user predictions
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        [HttpGet]
        [Route("{id}/{year}/{month}")]
        public IEnumerable<PredictionDailySummary> Daily(string id, int year, int month)
        {
            if (string.IsNullOrEmpty(id)) return null;

            const int IgnoreYear = 0;
            var matches = _matchesRepo.GetMatchesByYear(year).Where(it => it.FilterDateYear != IgnoreYear).Where(it => it.FilterDateMonth == month).ToList();
            var predictionQry = getPredictions(id, matches);

            var result = new List<PredictionDailySummary>();
            var days = matches.Where(it => it.FilterDateDay != IgnoreYear).Select(it => it.FilterDateDay).Distinct().OrderByDescending(it => it);
            var matchQry = matches.Where(it => days.Contains(it.FilterDateDay));
            var teamIds = matchQry.Select(it => it.TeamAwayId).Union(matchQry.Select(it => it.TeamHomeId)).Distinct();
            var teams = _matchesRepo.GetTeamsByIds(teamIds).ToList();
            foreach (var day in days)
            {
                var todayMatches = matches.Where(it => it.FilterDateDay == day);
                var predictions = getPredictions(todayMatches, predictionQry);
                var predictionResults = (from prediction in predictions
                                         let matchId = prediction.id.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1]
                                         let match = todayMatches.First(it => it.id.Equals(matchId))
                                         let teamHome = teams.FirstOrDefault(it=>it.id == match.TeamHomeId)
                                         where teamHome != null
                                         let teamAway = teams.FirstOrDefault(it => it.id == match.TeamAwayId)
                                         where teamAway != null
                                         select new PredictionDailyDetail
                                         {
                                             GainPoints = prediction.ActualPoints,
                                             IsMatchFinish = prediction.CompletedDate.HasValue,
                                             IsPredictionDraw = string.IsNullOrEmpty(prediction.PredictionTeamId),
                                             IsPredictionTeamHome = string.IsNullOrEmpty(prediction.PredictionTeamId) ? false : prediction.PredictionTeamId.Equals(teamHome.id),
                                             IsPredictionTeamAway = string.IsNullOrEmpty(prediction.PredictionTeamId) ? false : prediction.PredictionTeamId.Equals(teamAway.id),
                                             TeamHomeName = teamHome.Name,
                                             TeamAwayName = teamAway.Name,
                                             TeamHomeScore = match.TeamHomeScore,
                                             TeamAwayScore = match.TeamAwayScore,
                                         }).ToList();

                var date = new DateTime(year, month, day);
                var totalPoints = predictions.Sum(it => it.ActualPoints);
                result.Add(new PredictionDailySummary { Date = date, TotalPoints = totalPoints, PredictionResults = predictionResults });
            }
            return result.OrderByDescending(it => it.Date);
        }

        private IEnumerable<Prediction> getPredictions(string id, IEnumerable<Match> matches)
        {
            const int MaximumDataElements = 2;
            var predictionQry = from prediction in _predictionRepo.GetUserPredictions().ToList()
                                let data = prediction.id.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                                where data.Any() && data.Count() == MaximumDataElements
                                let userId = data[0]
                                let matchId = data[1]
                                where matches.Any(it => it.id.Equals(matchId))
                                where userId.Equals(id)
                                select prediction;
            return predictionQry;
        }
        private IEnumerable<Prediction> getPredictions(IEnumerable<Match> matches, IEnumerable<Prediction> predictions)
        {
            var predictionQry = from prediction in predictions
                                let data = prediction.id.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                                let matchId = data[1]
                                where matches.Any(it => it.id.Equals(matchId))
                                where prediction.CompletedDate.HasValue
                                select prediction;
            return predictionQry;
        }
    }
}
