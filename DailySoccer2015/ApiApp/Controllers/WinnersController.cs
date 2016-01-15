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
    /// Winners API
    /// </summary>
    public class WinnersController : ApiController
    {
        private IRewardRepository _rewardRepo;
        private IAccountRepository _accountRepo;

        /// <summary>
        /// Initialize Winners API
        /// </summary>
        /// <param name="rewardRepo">Reward repository</param>
        /// <param name="accountRepo">Account repository</param>
        public WinnersController(IRewardRepository rewardRepo, IAccountRepository accountRepo)
        {
            _rewardRepo = rewardRepo;
            _accountRepo = accountRepo;
        }

        // POST: api/Winners
        /// <summary>
        /// Randrom the reward winners
        /// </summary>
        public void Post()
        {
            const int RequiredMinimumOrderedCoupons = 1;
            var userprofiles = _accountRepo.GetAllUserProfiles()
                .Where(it => it.PreviousOrderedCoupon >= RequiredMinimumOrderedCoupons)
                .ToList();

            var random = new Random();
            var rewardGroup = _rewardRepo.GetLastCompletedRewardGroup();
            var rewards = _rewardRepo.GetRewardsByRewardGroupId(rewardGroup.id)
                .Where(it => it.Amount >= RequiredMinimumOrderedCoupons)
                .OrderBy(it => it.OrderedNo)
                .ToList();

            var now = DateTime.Now;
            var winners = new List<Winner>();
            foreach (var reward in rewards)
            {
                var availableRemainingAmount = userprofiles.Sum(it => it.PreviousOrderedCoupon);
                var shouldContinue = availableRemainingAmount >= RequiredMinimumOrderedCoupons;
                if (!shouldContinue) break;

                var isAvailableAtAll = availableRemainingAmount >= reward.Amount;
                var availableAmount = isAvailableAtAll ? reward.Amount : availableRemainingAmount;

                while (availableAmount-- > 0)
                {
                    var availableProfiles = userprofiles.Where(it => it.PreviousOrderedCoupon >= RequiredMinimumOrderedCoupons);
                    var randomUserIndex = random.Next(0, availableProfiles.Count() - 1);
                    var profile = userprofiles[randomUserIndex];
                    profile.PreviousOrderedCoupon--;
                    winners.Add(new Winner
                    {
                        id = Guid.NewGuid().ToString().Replace("-", string.Empty),
                        ReferenceCode = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper().Substring(0, 7),
                        RewardId = reward.id,
                        UserId = profile.id,
                        CreatedDate = now,
                    });
                }
            }

            if (winners.Any()) _rewardRepo.CreateNewWinners(winners);
        }

        // PUT: api/Winners
        /// <summary>
        /// Force end the current reward group
        /// </summary>
        public void Put()
        {
            var userprofiles = _accountRepo.GetAllUserProfiles().ToList();
            userprofiles.ForEach(it => _accountRepo.UpdateProfileByEndedCurrentRewardGroup(it.id, it.OrderedCoupon));
        }
    }
}
