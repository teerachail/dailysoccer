using ApiApp.Controllers;
using ApiApp.Models;
using ApiApp.Repositories;
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

            var req = new BuyCouponRequest { UserId = userId, BuyAmount = buyAmount };
            var couponCtrl = ScenarioContext.Current.Get<CouponsController>();
            var result = couponCtrl.Post(req);
            ScenarioContext.Current.Set(result);
        }
        
        [Then(@"ระบบทำการซื้อคูปอง '(.*)' ใบ ให้กับ UserId: '(.*)' พร้อมกับลดแต้มเหลือ Points: '(.*)'")]
        public void ThenระบบทำการซอคปองใบใหกบUserIdพรอมกบลดแตมเหลอPoints(int coupon, string userId, int points)
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Verify(dac => dac.UpdateFromBuyCoupons(It.Is<string>(it => it == userId), It.Is<int>(it => it == points), It.Is<int>(it => it == coupon)), Times.Exactly(1));
        }
    }
}
