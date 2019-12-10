using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    public class DisplayCase
    {
        
        [JsonProperty("total_autoreplied")]
        public int AutoRepliedCount { get; set; }

        [JsonProperty("total_close_case")]
        public int ClosedCasesCount { get; set; }

        [JsonProperty("total_open_case_data")]
        public List<OpenCase> OpenCases { get; set; }

        [JsonProperty("total_open_case")]
        public int OpenedCasesCount { get; set; }

        [JsonProperty("close_case_data")]
        public List<ClosedCase> ClosedCases { get; set; }

        [JsonProperty("total_autoreplied_data")]
        public List<OpenCase> AutoRepliedCases { get; set; }

        //[JsonProperty("total_close_case_today")]
        //public int CasesClosedToday { get; set; }

        //[JsonProperty("total_open_case_today")]
        //public int CasesOpenToday { get; set; }
        //[JsonProperty("open_case_today_data")]
        //public ObservableCollection<Case> OpenCasesToday { get; set; }
    }

    public class DisplaySentiments
    {
        [JsonProperty("data")]
        public List<Sentiment> Data { get; set; }
    }
}
