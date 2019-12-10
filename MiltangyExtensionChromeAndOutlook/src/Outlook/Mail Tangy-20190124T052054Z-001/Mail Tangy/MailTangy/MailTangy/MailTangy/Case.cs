using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    public class Case
    {
        //":[{"caseSubject":"Request For Service Invoice",
        //    "contactEmail":"khushbumehta102 @gmail.com",
        //    "caseNumber":"00001145",
        //    "caseStatus":"New",
        //    "contactName":"khusbhu mehta",
        //    "lightning_URL":"https://ap5.salesforce.com/one/one.app?source=aloha#/sObject/5007F000008TACKQA4",
        //    "classic_URL":"https://ap5.salesforce.com/5007F000008TACKQA4",
        //    "dateTimeOpened":"2017-12-07T08:31:27.000Z"}
        [JsonProperty("caseSubject")]
        public string CaseSubject { get; set; }

        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty("caseNumber")]
        public string CaseNumber { get; set; }

        [JsonProperty("caseStatus")]
        public string CaseStatus { get; set; }

        [JsonProperty("contactName")]
        public string ContactName { get; set; }

        [JsonProperty("lightning_URL")]
        public string Lightning_URL { get; set; }

        [JsonProperty("classic_URL")]
        public string Classic_URL { get; set; }

        [JsonProperty("Sentiment_Output")]
        public string Sentiment_Output { get; set; }

        private string summ;
        public string Summerizer_Output {
            get { return summ; }
            set
            {
                if (value == "")
                    summ = null;
                else
                    summ = value;
            }
        }

    }

    public class Feeling
    {
        [JsonProperty("polarity")]
        public string Polarity { get; set; }
    }
    public class OutputSummary
    {
        [JsonProperty("summery")]
        public string Summary { get; set; }
    }
    public class OpenCase:Case
    {
        public bool IsSelectedCase { get; set; }
        private string dateTimeOpened;
        [JsonProperty("dateTimeOpened")]
        public string DateTimeOpened
        {
            get
            {
                try
                {
                    DateTime theDate = DateTime.Parse(dateTimeOpened);
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
                dateTimeOpened = value;
            }
        }
            
    }
    public class ClosedCase : Case
    {
        private string dateTimeClosed;
        [JsonProperty("dateTimeClosed")]

        public string DateTimeClosed
        {
            get
            {
                try
                {
                    DateTime theDate = DateTime.Parse(dateTimeClosed);
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
                dateTimeClosed = value;
            }
        }
    }
    public class Sentiment
    {
        //":[{"id":"5a28fc5f65224be387e4738d",
        //    "from":"khushbu mehta ",
        //    "to":"ram.gupta9964 @gmail.com",
        //    "subject":"Request For Service Invoice",
        //    "Sentiment_Output":"negative"}

        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("to")]
        public string To { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("sentiment")]
        public string Sentiment_Output { get; set; }
    }

    public class AutoRepliedCase
    {
        //":[{"id":"5a0451346522a412624ec5da",
        //    "from":"khushbu mehta ",
        //    "to":"ram.gupta9964 @gmail.com",
        //    "subject":"Detail of Utility Service",
        //    "seen":"1",
        //    "receiveddate":"Thu Nov 09 08:10:21 UTC 2017",
        //    "case_no":"00001126",
        //    "auto_nlp_reply":"YES"
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

        [JsonProperty("receiveddate")]
        public string ReceivedDate { get; set; }
        [JsonProperty("case_no")]
        public string Case_no { get; set; }

        [JsonProperty("auto_nlp_reply")]
        public string Auto_NLP_Reply { get; set; }

    }
}
