using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.WorkstationDataReader.ConsoleApp
{
    public class StationInfo
    {
        public string StationName { get; set; }
        public string Active { get; set; }
        public bool LicenseBasicStation { get; set; }
        public string MacAddress { get; set; }
        public string Workgroup { get; set; }
    }
}
