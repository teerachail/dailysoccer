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
    public class LeagueController : ApiController
    {
        public IEnumerable<League> Get()
        {
            var client = new MongoClient("mongodb://MongoLab-4o:UMOcc359jl3WoTatREpo9qAAEGFL87uwoUWVyfusDUk-@ds056288.mongolab.com:56288/MongoLab-4o");
            var database = client.GetDatabase("MongoLab-4o");

            var leagueCollection = database.GetCollection<League>("xdailysoccer.league");
            var league = leagueCollection.Find(it => true).ToList();
            return league;
        }
    }
}
