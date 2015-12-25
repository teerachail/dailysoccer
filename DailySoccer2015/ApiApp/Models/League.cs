using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    public class League
    {
        public ObjectId Id { get; set; }
        public int LeagueId { get; set; }
        public string Name { get; set; }
    }
}
