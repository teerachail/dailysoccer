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
    /// Predictions API
    /// </summary>
    [RoutePrefix("api/predictions")]
    public class PredictionsController : ApiController
    {
        private IMatchesRepository _matchesRepo;
        private IPredictionRepository _predictionRepo;

        /// <summary>
        /// Initialize Prediction API
        /// </summary>
        /// <param name="matchesRepo">Matches repository</param>
        /// <param name="predictionRepo">Prediction repository</param>
        public PredictionsController(IMatchesRepository matchesRepo, IPredictionRepository predictionRepo)
        {
            _matchesRepo = matchesRepo;
            _predictionRepo = predictionRepo;
        }

        // GET: api/prediction/{user-id}/30/12/2015
        /// <summary>
        /// Get Prediction by day
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="day">Filter by day</param>
        /// <returns>Predictions data</returns>
        [HttpGet]
        [Route("{id}/{day}")]
        public IEnumerable<PredictionInformation> Get(string id, int day)
        {
            return GetPredictions(id, day);
        }

        // PUT: api/prediction/{user-id}
        /// <summary>
        /// Update user's prediction
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="value">Request body</param>
        [HttpPut]
        [Route("{id}")]
        public IEnumerable<PredictionInformation> Put(string id, PredictionRequest value)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id) && value != null && !string.IsNullOrEmpty(value.MatchId);
            if (!areArgumentsValid) return null;

            var selectedMatch = _matchesRepo.GetMatchById(value.MatchId);
            if (selectedMatch == null || selectedMatch.StartedDate.HasValue) return null;

            if (value.IsCancel) _predictionRepo.CancelUserPrediction(id, value.MatchId);
            else
            {
                var predictionPoints = string.IsNullOrEmpty(value.TeamId) ? selectedMatch.DrawPoints :
                    value.TeamId.Equals(selectedMatch.TeamHomeId) ? selectedMatch.TeamHomePoint : selectedMatch.TeamAwayPoint;
                _predictionRepo.SetUserPrediction(id, value.MatchId, value.TeamId, predictionPoints, DateTime.Now);
            }
            var now = DateTime.Now;
            return GetPredictions(id, now.Date.Day);
        }

        /// <summary>
        /// Get Prediction by day
        /// </summary>
        /// <param name="id"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private IEnumerable<PredictionInformation> GetPredictions(string id, int day)
        {
            var fromDate = DateTime.Now.AddDays(-3);
            var toDate = DateTime.Now.AddDays(3);
            var dateRange = Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                                      .Select(d => fromDate.AddDays(d));

            var selectedDate = dateRange.FirstOrDefault(it => it.Date.Day == day);
            if (selectedDate == null) return null;

            var prediction = _predictionRepo.GetUserPredictions();

            var splitSeparetor = '-';
            var userIdPosition = 0;
            var matchIdPosition = 1;
            var selectedPredictions = from predict in prediction.Where(it => it.id.Split(splitSeparetor)[userIdPosition] == id && it.CreatedDate.Date == selectedDate.Date)
                                      let matchId = predict.id.Split(splitSeparetor)[matchIdPosition]
                                      let selectedMatch = _matchesRepo.GetMatchById(matchId)
                                      let isPredictTeamHome = selectedMatch.TeamHomeId == predict.PredictionTeamId
                                      let isPredictTeamAway = selectedMatch.TeamAwayId == predict.PredictionTeamId
                                      let isPredictDraw = string.IsNullOrEmpty(predict.PredictionTeamId)
                                      select new PredictionInformation
                                      {
                                          MatchId = matchId,
                                          IsPredictionTeamHome = isPredictTeamHome,
                                          IsPredictionTeamAway = isPredictTeamAway,
                                          IsPredictionDraw = isPredictDraw,
                                          PredictionPoints = predict.PredictionPoints
                                      };
            return selectedPredictions;
        }
    }
}
