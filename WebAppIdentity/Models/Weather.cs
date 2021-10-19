using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Models
{
    public class Weather
    {
        public string info { get; set; }
        public string infocode { get; set; }
        public Lives[] lives { get; set; }
    }
    public class Lives
    {
        public string province { get; set; }
        public string city { get; set; }
        public string adcode { get; set; }
        public int temperature { get; set; }
        public string reporttime { get; set; }
    }
}
