using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DailySoccer
{
    [Binding]
    public sealed class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenarioBlock("mock")]
        public void BeforeScenarioBlock()
        {
            if (ScenarioContext.Current.CurrentScenarioBlock == ScenarioBlock.Given)
            {
                var mocks = new Moq.MockRepository(Moq.MockBehavior.Strict);
                ScenarioContext.Current.Set(mocks);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
