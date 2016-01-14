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
    /// Leagues API
    /// </summary>
    public class LeaguesController : ApiController
    {
        private IMatchesRepository _repo;

        /// <summary>
        /// Initialize Leagues API
        /// </summary>
        /// <param name="repo">Matches repository</param>
        public LeaguesController(IMatchesRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Leagues
        /// <summary>
        /// Get all leagues
        /// </summary>
        [HttpGet]
        public IEnumerable<League> Get()
        {
            var leagues = _repo.GetAllLeagues().ToList();
            return leagues;
        }
    }
}
