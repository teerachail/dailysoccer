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
            mockPredictionRepo.Setup(dac => dac.CancelUserPrediction(It.IsAny<string>(), It.IsAny<string>()));

            const string NULL = "NULL";
            userId = userId == NULL ? null : userId;
            matchId = matchId == NULL ? null : matchId;
            teamId = teamId == NULL ? null : teamId;
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
                It.IsAny<DateTime>()
                ), Times.Exactly(1));
        }

        [Then(@"ระบบบันทึกการยกเลิกการทายผลของ UserId: '(.*)', MatchId: '(.*)'")]
        public void ThenระบบบนทกการยกเลกการทายผลของUserIdMatchId(string userId, string matchId)
        {
            var mockPredictionRepo = ScenarioContext.Current.Get<Moq.Mock<IPredictionRepository>>();
            mockPredictionRepo.Verify(dac => dac.CancelUserPrediction(
                It.Is<string>(it => it == userId),
                It.Is<string>(it => it == matchId)),
                Times.Exactly(1));
        }

        [Then(@"ระบบไม่บันทึกการยกเลิกการทายผล")]
        public void Thenระบบไมบนทกการยกเลกการทายผล()
        {
            var mockPredictionRepo = ScenarioContext.Current.Get<Moq.Mock<IPredictionRepository>>();
            mockPredictionRepo.Verify(dac => dac.CancelUserPrediction(
                It.IsAny<string>(),
                It.IsAny<string>()
                ), Times.Never());
        }

        [Then(@"ระบบไม่ทำการบันทึกการทายผล")]
        public void Thenระบบไมทำการบนทกการทายผล()
        {
            var mockPredictionRepo = ScenarioContext.Current.Get<Moq.Mock<IPredictionRepository>>();
            mockPredictionRepo.Verify(dac => dac.SetUserPrediction(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>()
                ), Times.Never);
        }
    }
}
