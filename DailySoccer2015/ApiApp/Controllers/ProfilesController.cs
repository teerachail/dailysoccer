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
    /// Profiles API
    /// </summary>
    [RoutePrefix("api/profiles")]
    public class ProfilesController : ApiController
    {
        private IAccountRepository _accountRepo;

        /// <summary>
        /// Initialize Profiles API
        /// </summary>
        /// <param name="accountRepo">Account repository</param>
        public ProfilesController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        // GET: api/Profiles/5
        /// <summary>
        /// Get user profile by user id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public UserProfile Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var userProfile = _accountRepo.GetUserProfiles().FirstOrDefault(it => it.id.Equals(id));
            return userProfile;
        }
    }
}
