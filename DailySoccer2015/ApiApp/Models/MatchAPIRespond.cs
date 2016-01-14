using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลตอบกลับจาก API
    /// </summary>
    public class MatchAPIRespond
    {
        #region Properties

        public int APIVersion { get; set; }
        public int APIRequestsRemaining { get; set; }
        public bool DeveloperAuthentication { get; set; }
        public List<MatchAPIInformation> matches { get; set; }
        public string Action { get; set; }
        public string ComputationTime { get; set; }
        public string IP { get; set; }
        public string ERROR { get; set; }
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }

        #endregion Properties
    }
}
