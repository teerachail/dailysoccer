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
    /// Teams API
    /// </summary>
    [RoutePrefix("api/teams")]
    public class TeamsController : ApiController
    {
        private IMatchesRepository _repo;

        /// <summary>
        /// Initialize Teams API
        /// </summary>
        /// <param name="repo">Matches repository</param>
        public TeamsController(IMatchesRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Teams/5
        /// <summary>
        /// Get teams by league id
        /// </summary>
        /// <param name="id">League id</param>
        [HttpGet]
        public IEnumerable<Team> Get(string id)
        {
            return _repo.GetTeams().Where(it => it.LeagueId.Equals(id));
        }
    }
}
