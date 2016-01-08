using ApiApp.Controllers;
using ApiApp.Models;
using ApiApp.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TechTalk.SpecFlow;

namespace DailySoccer.Specs.Steps
{
    [Binding]
    public class BuyCouponSteps
    {
        [When(@"Call POST api/Coupons UserId: '(.*)', BuyAmount: '(.*)'")]
        public void WhenCallPOSTApiCouponsUserIdBuyAmount(string userId, int buyAmount)
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Setup(dac => dac.UpdateFromBuyCoupons(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            const string NULLValue = "NULL";
            userId = userId == NULLValue ? null : userId;
            var req = new BuyCouponRequest { UserId = userId, BuyAmount = buyAmount };
            var couponCtrl = ScenarioContext.Current.Get<CouponsController>();
            var result = couponCtrl.Post(req);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"ระบบอัพเดท UserId: '(.*)' ว่าเหลือ Points: '(.*)' และมีจำนวนคูปอง '(.*)' ใบ")]
        public void ThenระบบอพเดทUserIdวาเหลอPointsและมจำนวนคปองใบ(string userId, int points, int coupon)
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Verify(dac => dac.UpdateFromBuyCoupons(It.Is<string>(it => it == userId), It.Is<int>(it => it == points), It.Is<int>(it => it == coupon)), Times.Exactly(1));
        }

        [Then(@"ระบบไม่ทำการซื้อคูปอง")]
        public void Thenระบบไมทำการซอคปอง()
        {
            var actual = ScenarioContext.Current.Get<BuyCouponRespond>();
            Assert.IsFalse(actual.IsSuccess, "Buy IsSuccess");
            Assert.IsFalse(string.IsNullOrEmpty(actual.ErrorMessage), "Buy ErrorMessage");

            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Verify(dac => dac.UpdateFromBuyCoupons(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never());
        }
    }
}
