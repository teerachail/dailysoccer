﻿using ApiApp.Models;
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
        /// 
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="day">Filter by day</param>
        /// <param name="month">Filter by month</param>
        /// <param name="year">Filter by year</param>
        /// <returns>Predictions data</returns>
        [HttpGet]
        [Route("{id}/{day}/{month}/{year}")]
        public IEnumerable<PredictionInformation> Get(string id, int day, int month, int year)
        {
            var selectedDate = new DateTime(year, month, day);
            var prediction = _predictionRepo.GetUserPredictions();
            var match = _matchesRepo.GetMatches();
            var selectedPredictions = from predict in prediction.Where(it => it.CreatedDate == selectedDate)
                                      let selectedMatch = match.First(it => it.id == predict.MatchId)
                                      let isPredictTeamHome = selectedMatch.TeamHomeId == predict.PredictionTeamId
                                      let isPredictTeamAway = selectedMatch.TeamAwayId == predict.PredictionTeamId
                                      let isPredictDraw = string.IsNullOrEmpty(predict.PredictionTeamId)
                                      select new PredictionInformation
                                      {
                                           MatchId = predict.MatchId,
                                           IsPredictionTeamHome = isPredictTeamHome,
                                           IsPredictionTeamAway = isPredictTeamAway,
                                           IsPredictionDraw = isPredictDraw,
                                           PredictionPoints = predict.PredictionPoints
                                      };
            return selectedPredictions;
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
