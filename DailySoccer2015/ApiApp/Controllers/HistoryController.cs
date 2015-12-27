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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<PredictionMonthlySummary> Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var now = DateTime.Now;
            var matches = _matchesRepo.GetMatches()
                .Where(it => it.BeginDate.Year == now.Year)
                .ToList();

            const int MaximumDataElements = 2;
            var predictionQry = from prediction in _predictionRepo.GetUserPredictions()
                                let data = prediction.id.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                                where data.Any() && data.Count() == MaximumDataElements
                                let userId = data[0]
                                let matchId = data[1]
                                where matches.Any(it => it.id.Equals(matchId))
                                where userId.Equals(id)
                                select prediction;

            var result = new List<PredictionMonthlySummary>();
            var months = matches.Select(it => it.BeginDate.Month).Distinct().OrderByDescending(it => it);
            foreach (var month in months)
            {
                var matchIdQry = matches.Where(it => it.BeginDate.Month == month).Select(it => it.id);
                var totalPoints = (from prediction in predictionQry
                                   let data = prediction.id.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                                   let matchId = data[1]
                                   where matchIdQry.Any(it => it.Equals(matchId))
                                   where prediction.CompletedDate.HasValue
                                   select prediction).Sum(it => it.ActualPoints);

                var date = new DateTime(now.Year, month, 1);
                result.Add(new PredictionMonthlySummary { Date = date, TotalPoints = totalPoints });
            }

            return result;
        }
    }
}
