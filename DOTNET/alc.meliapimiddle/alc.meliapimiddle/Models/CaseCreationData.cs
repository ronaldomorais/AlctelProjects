using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alc.meliapimiddle.Models
{
    public class CaseCreationData
    {
        public string pin { get; set; }
        public string secret { get; set; }
        public string type { get; set; }
        public int ivr_option { get; set; }
        public string callid { get; set; }
        public string provider { get; set; }
    }
}
