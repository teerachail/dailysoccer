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
    /// Coupons API
    /// </summary>
    [RoutePrefix("api/coupons")]
    public class CouponsController : ApiController
    {
        private IRewardRepository _rewardRepo;
        private IAccountRepository _accountRepo;

        /// <summary>
        /// Initialize Coupons API
        /// </summary>
        /// <param name="repo">Reward repository</param>
        /// <param name="accountRepo">Account repository</param>
        public CouponsController(IRewardRepository repo, IAccountRepository accountRepo)
        {
            _rewardRepo = repo;
            _accountRepo = accountRepo;
        }

        // GET: api/coupons/summary/{user-id}
        /// <summary>
        /// Get user profile by user's id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet]
        [Route("summary/{id}")]
        public CouponSummaryRespond Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var userProfile = _accountRepo.GetUserProfileById(id);
            if (userProfile == null) return new CouponSummaryRespond();

            return new CouponSummaryRespond
            {
                RemainingPoints = userProfile.Points,
                OrderedCoupons = userProfile.OrderedCoupon
            };
        }

        // POST: api/Coupons
        /// <summary>
        /// Buy coupon
        /// </summary>
        [HttpPost]
        [Route("buy")]
        public BuyCouponRespond Post(BuyCouponRequest value)
        {
            const int MinimumBuyAmount = 1;
            var isArgumentValid = value != null && !string.IsNullOrEmpty(value.UserId) && value.BuyAmount >= MinimumBuyAmount;
            if (!isArgumentValid) return new BuyCouponRespond { ErrorMessage = "คำขอสั่งซื้อไม่ถูกต้อง" };

            var userProfile = _accountRepo.GetUserProfileById(value.UserId);
            var isUserProfileValid = userProfile != null
                && userProfile.IsFacebookVerified
                && userProfile.VerifiedPhoneDate.HasValue
                && !string.IsNullOrEmpty(userProfile.PhoneNo)
                && !string.IsNullOrEmpty(userProfile.VerifierCode);
            if (!isUserProfileValid) return new BuyCouponRespond { ErrorMessage = "ข้อมูลผู้ใช้ไม่ถูกต้อง" };

            var currentRewardGroup = _rewardRepo.GetCurrentRewardGroups();
            if (currentRewardGroup == null) return new BuyCouponRespond { ErrorMessage = "ยังไม่สามารถสั่งซื้อได้ในช่วงเวลานี้" };

            var requiredPoints = currentRewardGroup.RequiredPoints * value.BuyAmount;
            var arePointsEnough = userProfile.Points >= requiredPoints;
            if (!arePointsEnough) return new BuyCouponRespond { ErrorMessage = "แต้มที่มีไม่เพียงพอ" };

            var remainingPoints = userProfile.Points - requiredPoints;
            var orderedCoupons = userProfile.OrderedCoupon + value.BuyAmount;
            _accountRepo.UpdateFromBuyCoupons(value.UserId, remainingPoints, orderedCoupons);

            return new BuyCouponRespond
            {
                IsSuccess = true,
                AnnounceableDate = currentRewardGroup.ExpiredDate
            };
        }
    }
}
