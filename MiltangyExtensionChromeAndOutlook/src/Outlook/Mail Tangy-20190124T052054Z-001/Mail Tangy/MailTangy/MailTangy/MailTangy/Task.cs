using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    public class Task
    {
        public string AssignedTo { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }

        public string Name { get; set; }
        public string DueDate { get; set; }
        public string RelatedTo { get; set; }
        public string Priority { get; set; }

        public string CreatedBy { get; set; }
        public string lastModifiedBy { get; set; }
    }
    public class Tasks
    {
        public int Number_of_records { get; set; }
        public List<Task> taskdata { get; set; }
    }
}
