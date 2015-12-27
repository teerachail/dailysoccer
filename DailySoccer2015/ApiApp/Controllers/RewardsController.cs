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
        /// <summary>
        /// Get current rewards
        /// </summary>
        [HttpGet]
        public IEnumerable<Reward> Get()
        {
            var now = DateTime.Now;
            var lastRewardGroup = _repo.GetRewardGroups().OrderBy(it => it.ExpiredDate).LastOrDefault();
            if (lastRewardGroup == null) return Enumerable.Empty<Reward>();

            var rewards = _repo.GetRewards().Where(it => it.RewardGroupId.Equals(lastRewardGroup.id)).ToList();
            return rewards;
        }

        // GET: api/Rewards/winners
        /// <summary>
        /// Get current winners
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("winners")]
        public IEnumerable<RewardWinner> winners()
        {
            var now = DateTime.Now;
            var lastRewardGroup = _repo.GetRewardGroups().OrderBy(it => it.ExpiredDate).LastOrDefault();
            if (lastRewardGroup == null) return Enumerable.Empty<RewardWinner>();

            var lastRewardQry = _repo.GetRewards().Where(it => it.RewardGroupId.Equals(lastRewardGroup.id));
            if (!lastRewardQry.Any()) return Enumerable.Empty<RewardWinner>();

            var winners = _repo.GetWinners();
            var rewardWinners = lastRewardQry.Select(reward =>
            new RewardWinner
            {
                id = reward.id,
                Amount = reward.Amount,
                Description = reward.Description,
                ImgPath = reward.ImgPath,
                ThumbImgPath = reward.ThumbImgPath,
                OrderedNo = reward.OrderedNo,
                Price = reward.Price,
                RewardGroupId = reward.RewardGroupId,
                Winners = winners.Where(it => it.RewardId == reward.id).Select(it => it.id).ToList()
            }).ToList();

            return rewardWinners;
        }

        // GET: api/Rewards/5
        /// <summary>
        /// Get my rewards
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet]
        [Route("myrewards/{id}")]
        public IEnumerable<MyReward> Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return Enumerable.Empty<MyReward>();

            var winners = _repo.GetWinners().Where(it => it.id.Equals(id)).ToList();
            if (!winners.Any()) return Enumerable.Empty<MyReward>();

            var rewards = _repo.GetRewards().Where(reward => winners.Any(it => it.RewardId.Equals(reward.id))).ToList();
            if (!rewards.Any()) return Enumerable.Empty<MyReward>();

            var myRewards = (from winner in winners
                             from reward in rewards
                             where winner.RewardId.Equals(reward.id)
                             select new MyReward
                             {
                                 id = reward.id,
                                 Description = reward.Description,
                                 OrderedNo = reward.OrderedNo,
                                 Price = reward.Price,
                                 ReferenceCode = winner.ReferenceCode,
                                 ThumbImgPath = reward.ThumbImgPath,
                                 RewardDate = winner.CreatedDate
                             }).ToList();

            return myRewards;
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
