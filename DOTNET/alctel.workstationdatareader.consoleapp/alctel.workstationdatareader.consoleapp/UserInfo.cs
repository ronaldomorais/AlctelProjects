using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.WorkstationDataReader.ConsoleApp
{
    public class UserInfo
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Active { get; set; }
        public string Workgroup { get; set; }

        public string Role { get; set; }
        public List<string> CountedLicenses { get; set; }

        public UserInfo()
        {
            CountedLicenses = new List<string>();
        }
    }
}
