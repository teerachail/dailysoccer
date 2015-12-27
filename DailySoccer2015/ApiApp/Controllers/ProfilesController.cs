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
        private ISMSSender _smsSender;

        /// <summary>
        /// Initialize Profiles API
        /// </summary>
        /// <param name="accountRepo">Account repository</param>
        /// <param name="smsSender">SMS Sender</param>
        public ProfilesController(IAccountRepository accountRepo, ISMSSender smsSender)
        {
            _accountRepo = accountRepo;
            _smsSender = smsSender;
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

        // PUT: api/Profiles/5/phoneno
        /// <summary>
        /// Update phone number
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="value">Request body</param>
        [HttpPut]
        [Route("{id}/phoneno")]
        public void Put(string id, VerificationPhonenoRequest value)
        {
            const int MinimumPhoneNumberDigits = 10;
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && value != null
                && !string.IsNullOrEmpty(value.PhoneNumber)
                && value.PhoneNumber.Length >= MinimumPhoneNumberDigits;
            if (!areArgumentsValid) return;

            var shipOneDigit = 1;
            var phoneNo = value.PhoneNumber;
            phoneNo = phoneNo.Replace("-", string.Empty);
            phoneNo = string.Format("+66{0}", phoneNo.Substring(shipOneDigit, phoneNo.Length - shipOneDigit));

            _accountRepo.ResetVerifiedPhoneNumber(id);
            var verifyCode = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper().Substring(0, 7);
            _accountRepo.SetVerifierPhoneNumber(id, phoneNo, verifyCode);
            _smsSender.Send(phoneNo, verifyCode);
        }
    }
}
