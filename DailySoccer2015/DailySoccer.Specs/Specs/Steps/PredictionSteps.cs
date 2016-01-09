using ApiApp.Controllers;
using ApiApp.Models;
using ApiApp.Repositories;
using Moq;
using System;
using TechTalk.SpecFlow;

namespace DailySoccer.Specs.Steps
{
    [Binding]
    public class PredictionSteps
    {
        [When(@"Call PUT api/prediction UserId: '(.*)', MatchId: '(.*)', TeamId: '(.*)', IsCancel: '(.*)'")]
        public void WhenCallPUTApiPredictionUserIdMatchIdTeamIdIsCancel(string userId, string matchId, string teamId, bool isCancel)
        {
            var mockPredictionRepo = ScenarioContext.Current.Get<Moq.Mock<IPredictionRepository>>();
            mockPredictionRepo.Setup(dac => dac.SetUserPrediction(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()));

            var body = new PredictionRequest
            {
                MatchId = matchId,
                TeamId = teamId,
                IsCancel = isCancel
            };
            var predictionCtrl = ScenarioContext.Current.Get<PredictionsController>();
            var result = predictionCtrl.Put(userId, body);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"ระบบทำการบันทึกการทายผลให้กับ UserId: '(.*)', MatchId: '(.*)', TeamId: '(.*)', PredictionPoints: '(.*)'")]
        public void ThenระบบทำการบนทกการทายผลใหกบUserIdMatchIdTeamIdPredictionPoints(string userId, string matchId, string teamId, int predictionPoints)
        {
            var mockPredictionRepo = ScenarioContext.Current.Get<Moq.Mock<IPredictionRepository>>();
            mockPredictionRepo.Verify(dac => dac.SetUserPrediction(
                It.Is<string>(it => it == userId),
                It.Is<string>(it => it == matchId),
                It.Is<string>(it => it == teamId),
                It.Is<int>(it => it == predictionPoints),
                It.IsAny<DateTime>()), Times.Exactly(1));
        }
    }
}
