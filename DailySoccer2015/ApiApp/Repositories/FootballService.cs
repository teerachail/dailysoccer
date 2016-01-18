using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiApp.Models;
using System.Net;
using RestSharp;
using System.Web.Configuration;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวเชื่อมต่อกับฟุตบอลเซอร์วิส
    /// </summary>
    public class FootballService : IFootballService
    {
        #region IFootballService members

        /// <summary>
        /// ดึงข้อมูลแมช์การแข่งขันจากรหัสลีก
        /// </summary>
        /// <param name="leagueId">รหัสลีก</param>
        /// <param name="fromDate">เริ่มดึงจากวันที่</param>
        /// <param name="toDate">ดึงถึงวันที่</param>
        public IEnumerable<MatchAPIInformation> GetMatchesByLeagueId(string leagueId, DateTime fromDate, DateTime toDate)
        {
            const string dateFormat = "dd.MM.yyyy";
            var footballAPIKey = WebConfigurationManager.AppSettings["footballAPIKey"];
            const string urlFormat = "/api/?Action=fixtures&APIKey={0}&comp_id={1}&from_date={2}&to_date={3}";
            var url = string.Format(urlFormat, footballAPIKey, leagueId, fromDate.ToString(dateFormat), toDate.ToString(dateFormat));
            const string ClientBaseURL = "http://football-api.com";
            var client = new RestClient(ClientBaseURL);
            var request = new RestRequest(url);
            var respond = client.Execute<MatchAPIRespond>(request);
            var error = respond.Data == null || respond.Data.matches == null;
            if (error) return Enumerable.Empty<MatchAPIInformation>();

            return respond.Data.matches;
        }

        #endregion IFootballService members
    }
}
