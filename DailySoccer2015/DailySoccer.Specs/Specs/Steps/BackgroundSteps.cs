using ApiApp.Controllers;
using ApiApp.Repositories;
using Moq;
using System;
using TechTalk.SpecFlow;

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
            var smsSender = mock.Create<ISMSSender>();
            ScenarioContext.Current.Set(accountRepo);
            ScenarioContext.Current.Set(smsSender);

            var profile = new ProfilesController(accountRepo.Object, smsSender.Object);
            ScenarioContext.Current.Set(profile);
        }
    }
}
