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

        // GET: api/profile/5
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

        // POST api/profile
        /// <summary>
        /// Create new guest account
        /// </summary>
        [HttpPost]
        public UserProfile Post()
        {
            var userId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            _accountRepo.CreateUserProfile(userId);
            var userProfile = _accountRepo.GetUserProfiles().FirstOrDefault(it => it.id.Equals(userId));
            return userProfile;
        }

        // POST api/profile/facebook
        /// <summary>
        /// Get or Tie Facebook account
        /// </summary>
        [HttpPost]
        [Route("facebook")]
        public UserProfile facebook(FacebookRequest value)
        {
            var areArgumentsValid = value != null && !string.IsNullOrEmpty(value.FacebookId);
            if (!areArgumentsValid) return null;

            var facebookAccount = _accountRepo.GetFacebookAccounts().FirstOrDefault(it => it.id.Equals(value.FacebookId));
            if (facebookAccount == null) return null;

            if (value.IsConfirmed)
            {
                var isArgumentValid = !string.IsNullOrEmpty(value.UserId);
                if (!isArgumentValid) return null;

                var userprofile = _accountRepo.GetUserProfiles().FirstOrDefault(it => it.id.Equals(value.UserId));
                if (userprofile == null) return null;

                _accountRepo.UntieFacebookAccount(value.FacebookId);
                _accountRepo.TieFacebookAccount(value.FacebookId, value.UserId);
                userprofile.IsFacebookVerified = true;
                return userprofile;
            }
            else
            {
                var userprofile = _accountRepo.GetUserProfiles().FirstOrDefault(it => it.id.Equals(value.FacebookId));
                return userprofile;
            }
        }

        // PUT: api/profile/0912345678/phoneno
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

            _accountRepo.ResetVerifiedPhoneNumber(id);
            var phoneNo = convertToThailandPhoneNoFormat(value.PhoneNumber);
            var verifyCode = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper().Substring(0, 7);
            _accountRepo.SetVerifierPhoneNumber(id, phoneNo, verifyCode);
            _smsSender.Send(phoneNo, verifyCode);
        }

        // PUT: api/profile/0912345678/vericode
        /// <summary>
        /// Request verifyphone number
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="value">Request body</param>
        [HttpPut]
        [Route("{id}/vericode")]
        public VerificationCodeRespond vericode(string id, VerificationCodeRequest value)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && value != null
                && !string.IsNullOrEmpty(value.PhoneNumber)
                && !string.IsNullOrEmpty(value.VerificationCode);
            if (!areArgumentsValid) return new VerificationCodeRespond();

            var userProfile = _accountRepo.GetUserProfiles().FirstOrDefault(it => it.id.Equals(id));
            if (userProfile == null) return new VerificationCodeRespond();

            var phoneNo = convertToThailandPhoneNoFormat(value.PhoneNumber);
            var isSuccess = userProfile.PhoneNo.Equals(phoneNo) && userProfile.VerifierCode.Equals(value.VerificationCode);
            if (!isSuccess) return new VerificationCodeRespond();
            _accountRepo.SetVerifiedPhoneNumberComplete(id, DateTime.Now);
            return new VerificationCodeRespond { IsSuccess = true };
        }

        // แปลงเบอร์โทรศัพท์ให้เป็นหมายเลขประเทศไทย
        private string convertToThailandPhoneNoFormat(string phoneNo)
        {
            const string ReplaceStarterPhoneNumber = "0";
            phoneNo = phoneNo.Replace("-", string.Empty);
            if (phoneNo.StartsWith(ReplaceStarterPhoneNumber))
            {
                const int shipOneDigit = 1;
                phoneNo = string.Format("+66{0}", phoneNo.Substring(shipOneDigit, phoneNo.Length - shipOneDigit));
            }
            return phoneNo;
        }
    }
}
