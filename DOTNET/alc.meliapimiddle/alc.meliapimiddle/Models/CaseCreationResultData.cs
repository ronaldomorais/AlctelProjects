using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alc.meliapimiddle.Models
{
    public class CaseCreationResultData
    {
        public int case_number { get; set; }
        public string case_number_splitted { get; set; }
        public string provider { get; set; }
        public bool from_faq { get; set; }
        public bool is_created { get; set; }
    }
}
