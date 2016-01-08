using ApiApp.Controllers;
using ApiApp.Models;
using ApiApp.Repositories;
using Moq;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Linq;
using System.Collections.Generic;

namespace DailySoccer.Specs.Steps
{
    [Binding]
    public class BackgroundSteps
    {
        [Given(@"Create mocking")]
        public void GivenCreateMocking()
        {
            var mock = ScenarioContext.Current.Get<MockRepository>();

            var accountRepo = mock.Create<IAccountRepository>();
            var rewardRepo = mock.Create<IRewardRepository>();
            var smsSender = mock.Create<ISMSSender>();
            ScenarioContext.Current.Set(accountRepo);
            ScenarioContext.Current.Set(rewardRepo);
            ScenarioContext.Current.Set(smsSender);

            var profile = new ProfilesController(accountRepo.Object, smsSender.Object);
            var coupon = new CouponsController(rewardRepo.Object, accountRepo.Object);
            ScenarioContext.Current.Set(profile);
            ScenarioContext.Current.Set(coupon);
        }

        [Given(@"Reward groups in the system are")]
        public void GivenRewardGroupsInTheSystemAre(Table table)
        {
            var rewardGroups = table.CreateSet<RewardGroup>().ToList();
            var mockRewardRepo = ScenarioContext.Current.Get<Moq.Mock<IRewardRepository>>();
            mockRewardRepo.Setup(dac => dac.GetCurrentRewardGroups())
                .Returns(() => rewardGroups.OrderByDescending(it => it.ExpiredDate).FirstOrDefault());
        }

        [Given(@"Facebook accounts in the system are")]
        public void GivenFacebookAccountsInTheSystemAre(Table table)
        {
            var facebookAccounts = table.CreateSet<FacebookAccount>();
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Setup(dac => dac.GetFacebookAccountsById(It.IsAny<string>()))
                .Returns<string>(id => facebookAccounts.FirstOrDefault(it => it.id == id));
        }

        [Given(@"UserProfile accounts in the system are")]
        public void GivenUserProfileAccountsInTheSystemAre(Table table)
        {
            var userprofileAccounts = table.CreateSet<UserProfile>().ToList();
            ScenarioContext.Current.Set(userprofileAccounts);
            var mockAccountRepo = ScenarioContext.Current.Get<Moq.Mock<IAccountRepository>>();
            mockAccountRepo.Setup(dac => dac.GetUserProfileById(It.IsAny<string>()))
                .Returns<string>(id => ScenarioContext.Current.Get<List<UserProfile>>().FirstOrDefault(it => it.id == id));
        }
    }
}
