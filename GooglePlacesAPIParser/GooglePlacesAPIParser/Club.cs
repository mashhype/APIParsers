using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlacesAPIParser
{
    class Club
    {

        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string id { get; set; }
        public string Type { get; set; }
        public string competitorKey { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string rating { get; set; }

        public string status { get; set; }
    }
}
