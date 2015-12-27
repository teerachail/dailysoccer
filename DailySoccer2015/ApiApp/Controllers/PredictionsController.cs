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

        // GET: api/prediction/{user-id}/{day}
        /// <summary>
        /// Get user's predictions by day
        /// </summary>
        /// <param name="id">USer id</param>
        /// <param name="day">Filter by day</param>
        [HttpGet]
        [Route("{id}/{day}")]
        public string Get(string id, int day)
        {
            // TODO: GET user's predictions
            throw new NotImplementedException();
        }

        // PUT: api/prediction/{user-id}
        /// <summary>
        /// Update user's prediction
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="value">Request body</param>
        [HttpPut]
        [Route("{id}")]
        public void Put(string id, PredictionRequest value)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id) && value != null && !string.IsNullOrEmpty(value.MatchId);
            if (!areArgumentsValid) return;

            var selectedMatch = _matchesRepo.GetMatches().FirstOrDefault(it => it.id.Equals(value.MatchId));
            if (selectedMatch == null || selectedMatch.StartedDate.HasValue) return;

            if (value.IsCancel) _predictionRepo.CancelUserPrediction(id, value.MatchId);
            else
            {
                var predictionPoints = string.IsNullOrEmpty(value.TeamId) ? selectedMatch.DrawPoints :
                    value.TeamId.Equals(selectedMatch.TeamHomeId) ? selectedMatch.TeamHomePoint : selectedMatch.TeamAwayPoint;
                _predictionRepo.SetUserPrediction(id, value.MatchId, value.TeamId, predictionPoints, DateTime.Now);
            }
        }
    }
}
