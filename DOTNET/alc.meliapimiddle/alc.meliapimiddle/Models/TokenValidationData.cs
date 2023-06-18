using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alc.meliapimiddle.Models
{
    public class TokenValidationData
    {
        public int pin { get; set; }
        public string secret { get; set; }
        public string type { get; set; }
    }
}
