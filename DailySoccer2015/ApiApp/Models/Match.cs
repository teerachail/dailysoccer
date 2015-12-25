using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    public class Match
    {
        public ObjectId Id { get; set; }
        public int TeamHomeId { get; set; }
        public int TeamHomeScore { get; set; }
        public int TeamHomePoint { get; set; }
        public int TeamAwayId { get; set; }
        public int TeamAwayScore { get; set; }
        public int TeamAwayPoint { get; set; }
        public DateTime BeginDate { get; set; }
        public Nullable<DateTime> StartedDate { get; set; }
        public Nullable<DateTime> CompletedDate { get; set; }
        public int LeagueId { get; set; }
    }
}
