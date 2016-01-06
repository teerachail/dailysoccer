using ApiApp.Controllers;
using ApiApp.Models;
using ApiApp.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Linq;

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

        [When(@"Call POST api/profile/facebook \[FacebookId: '(.*)', UserId: '(.*)', IsConfirmed: '(.*)']")]
        public void WhenCallPOSTApiProfileFacebookFacebookIdUserIdTrue(string facebookId, string userId, bool isConfirmed)
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Setup(dac => dac.CreateUserProfile(It.IsAny<string>())).Callback<string>(id =>
            {
                var userprofiles = ScenarioContext.Current.Get<List<UserProfile>>();
                userprofiles.Add(new UserProfile { id = id });
                ScenarioContext.Current.Set(userprofiles);
            });
            mockAccountRepo.Setup(dac => dac.TieFacebookAccount(It.IsAny<string>(), It.IsAny<string>()));
            mockAccountRepo.Setup(dac => dac.UntieFacebookAccount(It.IsAny<string>()));

            var profileCtrl = ScenarioContext.Current.Get<ProfilesController>();
            var result = profileCtrl.facebook(new FacebookRequest
            {
                FacebookId = facebookId,
                UserId = userId,
                IsConfirmed = isConfirmed
            });
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System not create new guest account")]
        public void ThenSystemNotCreateNewGuestAccount()
        {
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Verify(dac => dac.CreateUserProfile(It.IsAny<string>()), Times.Never());
        }

        [Then(@"system return account data are")]
        public void ThenSystemReturnAccountDataAre(Table table)
        {
            var expected = table.CreateSet<UserProfile>().First();
            var actual = ScenarioContext.Current.Get<UserProfile>();

            Assert.AreEqual(expected.Points, actual.Points, "Points");
            Assert.AreEqual(expected.OrderedCoupon, actual.OrderedCoupon, "OrderedCoupon");
            Assert.AreEqual(expected.PhoneNo, actual.PhoneNo, "PhoneNo");
            Assert.AreEqual(expected.VerifierCode, actual.VerifierCode, "VerifierCode");
            Assert.AreEqual(expected.FavouriteTeamId, actual.FavouriteTeamId, "FavouriteTeamId");
            Assert.AreEqual(expected.VerifiedPhoneDate, actual.VerifiedPhoneDate, "VerifiedPhoneDate");
            Assert.AreEqual(expected.IsFacebookVerified, actual.IsFacebookVerified, "IsFacebookVerified");
        }

        [Then(@"system return null")]
        public void ThenSystemReturnNull()
        {
            bool result = false;
            try
            {
                UserProfile actual = null;
                result = ScenarioContext.Current.TryGetValue<UserProfile>(out actual);
            }
            catch (Exception) { }

            if (result) Assert.Inconclusive();
        }
    }
}
