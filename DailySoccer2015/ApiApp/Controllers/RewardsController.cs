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
    /// Reward API
    /// </summary>
    [RoutePrefix("api/Rewards")]
    public class RewardsController : ApiController
    {
        private IRewardRepository _repo;

        /// <summary>
        /// Initialize Reward API
        /// </summary>
        /// <param name="repo">Reward repository</param>
        public RewardsController(IRewardRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Rewards
        public IEnumerable<Reward> Get()
        {
            var now = DateTime.Now;
            var lastRewardGroup = _repo.GetRewardGroups().OrderBy(it => it.ExpiredDate).LastOrDefault();
            if (lastRewardGroup == null) return Enumerable.Empty<Reward>();

            var rewards = _repo.GetRewards()
                .Where(it => it.RewardGroupId.Equals(lastRewardGroup.id));
            return rewards;
        }

        // GET: api/Rewards/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Rewards
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Rewards/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Rewards/5
        public void Delete(int id)
        {
        }
    }
}
