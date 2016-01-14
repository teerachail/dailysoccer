using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Models
{
    /// <summary>
    /// ข้อมูลแมช์ที่ได้จาก API
    /// </summary>
    public class MatchAPIInformation
    {
        #region Properties

        public string match_id { get; set; }
        public string match_comp_id { get; set; }
        public string match_date { get; set; }
        public string match_formatted_date { get; set; }
        public string match_status { get; set; }
        public string match_time { get; set; }
        public string match_commentary_available { get; set; }
        public string match_localteam_id { get; set; }
        public string match_localteam_name { get; set; }
        public int match_localteam_score { get; set; }
        public string match_visitorteam_id { get; set; }
        public string match_visitorteam_name { get; set; }
        public int match_visitorteam_score { get; set; }
        public string match_ht_score { get; set; }
        public string match_static_id { get; set; }
        public string match_season_beta { get; set; }
        public string match_week_beta { get; set; }
        public string match_venue_beta { get; set; }
        public string match_venue_id_beta { get; set; }
        public string match_venue_city_beta { get; set; }
        public string match_timer { get; set; }
        public string match_ft_score { get; set; }
        public string match_et_score { get; set; }

        public string LeagueName { get; set; }
        public int DifferentDay { get; set; }

        public string TimeZone { get; set; }

        #endregion Properties
    }
}
