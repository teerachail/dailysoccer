using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailySoccerBackOffice.Controllers
{
    public class WinnerController : Controller
    {
        // GET: Winner
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string EndCurrentRewardGroup()
        {
            callDailySoccerAPI(Method.PUT);
            return "Current reward group was ended.";
        }

        [HttpPost]
        public string RandomWinner()
        {
            callDailySoccerAPI(Method.POST);
            return "Random the winners was completed.";
        }

        private void callDailySoccerAPI(Method method)
        {
            const string ClientBaseURL = "http://dailysoccer-joker.azurewebsites.net";
            var client = new RestClient(ClientBaseURL);
            var request = new RestRequest("api/winners", method);
            var respond = client.Execute(request);
            var content = respond.Content;
        }
    }
}