using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    public class MailData
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("seen")]
        public string Seen { get; set; }

       
        private string receivedDate;
        [JsonProperty("receiveddate")]
        public string ReceivedDate
        {
           get
            {
                try
                {
                    DateTime theDate = DateTime.Parse(receivedDate);
                    // the string was successfully parsed into theDate
                    return theDate.ToString("MMM/dd/yyyy hh:mm tt");
                }
                catch (Exception)
                {
                    return "n/A";
                }
            }
            set
            {
                receivedDate = value; 
            }
        }

        [JsonProperty("auto_nlp_reply")]
        public string Auto_NLP_Reply { get; set; }

        [JsonProperty("attachfiles")]
        public bool HasAttachemnts { get; set; }

        [JsonProperty("attachpath")]
        public string[] AttachmentsPath { get; set; }

        [JsonProperty("reply_data")]
        public ReplyData MailReplyData { get; set; }

        [JsonProperty("Summerizer_Output")]
        public string Summerizer_Output { get; set; }
        
    }

    public  class ReplyData
    {
        [JsonProperty("attachfile")]
        public bool HasAttachments { get; set; }
        [JsonProperty("replytext")]
        public string ReplyText { get; set; }
        [JsonProperty("sentdate")]
        public string SentDate { get; set; }
        [JsonProperty("attachpath")]
        public string[] AttachmentsPath { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public  class LeadData:MailData
    {
        public string lead_no { get; set; }
    }

    public class CaseData : MailData
    {
        public string case_no { get; set; }
    }

    public class EmailBasedCaseData
    {
        [JsonProperty("leaddata")]
        public List<LeadData> LeadDataCollection { get; set; }

        [JsonProperty("casedata")]
        public List<CaseData> LeadCaseCollection { get; set; }
    }
}
