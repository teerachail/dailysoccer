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
    public class RegisterSteps
    {
        [When(@"Call POST api/profile")]
        public void WhenCallPOSTApiProfile()
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Setup(dac => dac.CreateUserProfile(It.IsAny<string>()));
            mockAccountRepo.Setup(dac => dac.GetUserProfileById(It.IsAny<string>()))
                .Returns<string>(it => new ApiApp.Models.UserProfile { id = it });

            var profileCtrl = ScenarioContext.Current.Get<ProfilesController>();
            var result = profileCtrl.Post();
            ScenarioContext.Current.Set(result);
        }
        
        [Then(@"System create new guest account")]
        public void ThenSystemCreateNewGuestAccount()
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Verify(dac => dac.CreateUserProfile(It.IsAny<string>()), Times.Exactly(1));
        }
        
        [Then(@"System return the new account to the caller")]
        public void ThenSystemReturnTheNewAccountToTheCaller()
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Verify(dac => dac.GetUserProfileById(It.IsAny<string>()), Times.Exactly(1));
        }

        [Then(@"the new account data should be newly account")]
        public void ThenTheNewAccountDataShouldBeNewlyAccount()
        {
            var actual = ScenarioContext.Current.Get<UserProfile>();
            Assert.AreEqual(0, actual.Points, "Points");
            Assert.AreEqual(0, actual.OrderedCoupon, "OrderedCoupon");
            Assert.IsNull(actual.PhoneNo, "PhoneNo");
            Assert.IsNull(actual.VerifierCode, "VerifierCode");
            Assert.IsNull(actual.FavouriteTeamId, "FavouriteTeamId");
            Assert.IsNull(actual.VerifiedPhoneDate, "VerifiedPhoneDate");
            Assert.IsFalse(actual.IsFacebookVerified, "IsFacebookVerified");
        }
    }
}
