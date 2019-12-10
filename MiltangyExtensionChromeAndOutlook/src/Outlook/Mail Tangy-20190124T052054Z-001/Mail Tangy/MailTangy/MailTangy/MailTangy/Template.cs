using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    public class Template
    {
        public string temp_name { get; set; }
        public string temp_txt { get; set; }
    }

    public class Templates
    {
        public List<Template> data { get; set; }
    }
}
