using repo = ApiApp.Repositories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    static class DiConfig
    {
        public static IContainer CreateContainer()
        {
            return new Container(c =>
            {
                c.For<repo.IRewardRepository>().Use<repo.RewardRepository>();
                c.For<repo.IAccountRepository>().Use<repo.AccountRepository>();
                c.For<repo.ISMSSender>().Use<repo.TwilioSMSSender>();
                c.For<repo.IPredictionRepository>().Use<repo.PredictionRepository>();
                c.For<repo.IMatchesRepository>().Use<repo.MatchesRepository>();
                c.For<repo.IAdvertisementsRepository>().Use<repo.AdvertisementsRepository>();
            });
        }
    }
}
