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
        }

        // PUT: api/Winners
        /// <summary>
        /// Force end the current reward group
        /// </summary>
        public void Put()
        {

        }
    }
}
