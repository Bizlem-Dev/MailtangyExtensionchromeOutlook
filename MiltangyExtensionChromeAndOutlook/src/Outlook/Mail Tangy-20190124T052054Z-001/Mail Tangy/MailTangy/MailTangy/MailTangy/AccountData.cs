using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy
{
    public class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Type { get; set; }
    }

    public class ContactData
    {
        public List<UserData> contactData { get; set; }
    }

    public class AccountData
    {
        public List<UserData> accountData { get; set; }
    }
}
