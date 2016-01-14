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
    public class SyncController : ApiController
    {
        private IMatchesRepository _matchRepo;
        private IAccountRepository _accountRepo;
        private IPredictionRepository _predictionRepo;

        /// <summary>
        /// Initialize Sync API
        /// </summary>
        /// <param name="matchRepo">Match repository</param>
        /// <param name="accountRepo">Account  repository</param>
        /// <param name="predictionRepo">Prediction  repository</param>
        public SyncController(IMatchesRepository matchRepo, IAccountRepository accountRepo, IPredictionRepository predictionRepo)
        {
            _matchRepo = matchRepo;
            _accountRepo = accountRepo;
            _predictionRepo = predictionRepo;
        }

        // GET: api/Sync
        /// <summary>
        /// Update all matches
        /// </summary>
        [HttpGet]
        public void Get()
        {
            // TODO: Not implement
            throw new NotImplementedException();
        }

        // GET: api/Sync/raw
        /// <summary>
        /// Get matches
        /// </summary>
        /// <param name="id">API key</param>
        [HttpGet]
        [Route("/raw")]
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
            // TODO: Not implement
            throw new NotImplementedException();
        }

        private string convertAPIMatch(int apiMatch)
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
            // TODO: Not implement
            throw new NotImplementedException();
        }
    }
}
