using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    class AutoReply
    {
        // {"to":"ashish.shukla9964@gmail.com","from":"vinayak varma <vinayak.varma10@gmail.com>",
        //"label":"CASE","auto_reply_status":"false",
        //"replySubject":"RE:Check Autoreplay","replyText":"","attachments":[{}]}

        public string to { get; set; }
        public string from { get; set; }
        public string label { get; set; }
        public string auto_reply_status { get; set; }
        public string replySubject { get; set; }
        public string replyText { get; set; }
        public List<EmailAttachment> attachments { get; set; }
    }

    public class EmailAttachment
    {
        public string fileName { get; set; }
        public string contentType { get; set; }
        public string data { get; set; }
    }
}
