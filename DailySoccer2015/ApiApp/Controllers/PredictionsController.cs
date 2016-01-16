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
        private IAccountRepository _accountRepo;

        /// <summary>
        /// Initialize Prediction API
        /// </summary>
        /// <param name="matchesRepo">Matches repository</param>
        /// <param name="predictionRepo">Prediction repository</param>
        /// <param name="accountRepo">Account repository</param>
        public PredictionsController(IMatchesRepository matchesRepo, IPredictionRepository predictionRepo, IAccountRepository accountRepo)
        {
            _matchesRepo = matchesRepo;
            _predictionRepo = predictionRepo;
            _accountRepo = accountRepo;
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
            return getPredictionsByDay(id, day);
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

            const int IgnoreDay = 0;
            var selectedMatch = _matchesRepo.GetMatchById(value.MatchId);
            var isMatchReadyForPrediction = selectedMatch != null
                && !string.IsNullOrEmpty(selectedMatch.FilterDate)
                && selectedMatch.FilterDateDay != IgnoreDay
                && !selectedMatch.StartedDate.HasValue
                && !selectedMatch.CompletedDate.HasValue
                && (selectedMatch.TeamAwayId.Equals(value.TeamId) || selectedMatch.TeamHomeId.Equals(value.TeamId) || string.IsNullOrEmpty(value.TeamId));
            if (!isMatchReadyForPrediction) return null;

            var selectedProfile = _accountRepo.GetUserProfileById(id);
            if (selectedProfile == null) return null;

            if (value.IsCancel) _predictionRepo.CancelUserPrediction(id, value.MatchId);
            else
            {
                var predictionPoints = string.IsNullOrEmpty(value.TeamId) ? selectedMatch.DrawPoints :
                    value.TeamId.Equals(selectedMatch.TeamHomeId) ? selectedMatch.TeamHomePoint : selectedMatch.TeamAwayPoint;
                var actualPredictionPoints = predictionPoints.HasValue ? predictionPoints.Value : 0;
                _predictionRepo.SetUserPrediction(id, value.MatchId, value.TeamId, actualPredictionPoints, DateTime.Now);
            }
            var now = DateTime.Now;
            return getPredictionsByDay(id, selectedMatch.FilterDateDay);
        }

        // Get Prediction by day
        private IEnumerable<PredictionInformation> getPredictionsByDay(string id, int day)
        {
            const int DayRange = 3;
            var fromDate = DateTime.Now.AddDays(-DayRange);
            var toDate = DateTime.Now.AddDays(DayRange);
            var dateRange = Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1).Select(d => fromDate.AddDays(d));
            var selectedDate = dateRange.FirstOrDefault(it => it.Date.Day == day);
            if (selectedDate == null) return null;

            var splitSeparetor = '-';
            var userIdPosition = 0;
            var matchIdPosition = 1;

            var predictions = _predictionRepo.GetUserPredictions()
                .Where(it => it.id.Split(splitSeparetor)[userIdPosition] == id)
                .ToList();
            var matches = _matchesRepo.GetMatchesByDate(selectedDate).ToList();

            var qry = from predict in predictions
                      let matchId = predict.id.Split(splitSeparetor)[matchIdPosition]
                      let selectedMatch = matches.FirstOrDefault(it => it.id == matchId)
                      where selectedMatch != null
                      let isPredictTeamHome = selectedMatch.TeamHomeId == predict.PredictionTeamId
                      let isPredictTeamAway = selectedMatch.TeamAwayId == predict.PredictionTeamId
                      let isPredictDraw = string.IsNullOrEmpty(predict.PredictionTeamId)
                      let isDisplayActualPoints = selectedMatch.CompletedDate.HasValue && predict.CompletedDate.HasValue
                      select new PredictionInformation
                      {
                          MatchId = matchId,
                          IsPredictionTeamHome = isPredictTeamHome,
                          IsPredictionTeamAway = isPredictTeamAway,
                          IsPredictionDraw = isPredictDraw,
                          PredictionPoints = isDisplayActualPoints ? predict.ActualPoints : predict.PredictionPoints
                      };
            var result = qry.ToList();
            return result;
        }
    }
}
