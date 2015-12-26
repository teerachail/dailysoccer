using ApiApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiApp.Controllers
{
    public class MatchesController : ApiController
    {
        /// <summary>
        /// GetAllMatch
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MatchInformation> Get(int day)
        {
            var client = new MongoClient("mongodb://MongoLab-4o:UMOcc359jl3WoTatREpo9qAAEGFL87uwoUWVyfusDUk-@ds056288.mongolab.com:56288/MongoLab-4o");
            var database = client.GetDatabase("MongoLab-4o");

            var matchCollection = database.GetCollection<Match>("xdailysoccer.match");
            var teamCollection = database.GetCollection<Team>("xdailysoccer.team");           

            var matches = matchCollection.Find(it => true).ToList();
            var teams = teamCollection.Find(it => true).ToList();

            var currentDay = DateTime.Now;
            var selectedMatch = from match in matches.Where(it => it.BeginDate.Day == day && it.BeginDate.Year == currentDay.Year)
                                let teamHomeName = teams.First(team => team.id == match.TeamHomeId).Name
                                let teamAwayName = teams.First(team => team.id == match.TeamAwayId).Name
                                select new MatchInformation
                                {
                                     TeamHomeName = teamHomeName,
                                     TeamAwayName = teamAwayName,
                                     TeamHomePoint = match.TeamHomePoint,
                                     TeamAwayPoint = match.TeamAwayPoint,
                                     TeamHomeScore = match.TeamHomeScore,
                                     TeamAwayScore = match.TeamAwayScore,
                                     BeginDate = match.BeginDate,
                                     StartedDate = match.StartedDate,
                                     CompletedDate = match.CompletedDate,
                                     LeagueId = match.LeagueId
                                };

            return selectedMatch.ToList();
        }
    }
}
