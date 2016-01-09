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
            var matchRepo = mock.Create<IMatchesRepository>();
            var predictionRepo = mock.Create<IPredictionRepository>();
            var smsSender = mock.Create<ISMSSender>();
            ScenarioContext.Current.Set(accountRepo);
            ScenarioContext.Current.Set(rewardRepo);
            ScenarioContext.Current.Set(matchRepo);
            ScenarioContext.Current.Set(predictionRepo);
            ScenarioContext.Current.Set(smsSender);

            var profile = new ProfilesController(accountRepo.Object, smsSender.Object);
            var coupon = new CouponsController(rewardRepo.Object, accountRepo.Object);
            var prediction = new PredictionsController(matchRepo.Object, predictionRepo.Object, accountRepo.Object);
            ScenarioContext.Current.Set(profile);
            ScenarioContext.Current.Set(coupon);
            ScenarioContext.Current.Set(prediction);
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

        [Given(@"Matches in the system are")]
        public void GivenMatchesInTheSystemAre(Table table)
        {
            var matches = table.CreateSet<ApiApp.Models.Match>().ToList();
            ScenarioContext.Current.Set(matches);
            var mockMatchRepo = ScenarioContext.Current.Get<Moq.Mock<IMatchesRepository>>();
            mockMatchRepo.Setup(dac => dac.GetMatchById(It.IsAny<string>()))
                .Returns<string>(id => ScenarioContext.Current.Get<List<ApiApp.Models.Match>>().FirstOrDefault(it => it.id == id));
            mockMatchRepo.Setup(dac => dac.GetMatchesByDate(It.IsAny<DateTime>()))
                .Returns<DateTime>(date => ScenarioContext.Current.Get<List<ApiApp.Models.Match>>().Where(it => it.BeginDate.Date == date.Date));
        }

        [Given(@"Predictions in the system are")]
        public void GivenPredictionsInTheSystemAre(Table table)
        {
            var predictions = table.CreateSet<Prediction>().ToList();
            ScenarioContext.Current.Set(predictions);
            var mockPredictionRepo = ScenarioContext.Current.Get<Moq.Mock<IPredictionRepository>>();
            mockPredictionRepo.Setup(dac => dac.GetUserPredictions())
                .Returns(ScenarioContext.Current.Get<List<Prediction>>());
        }
    }
}
