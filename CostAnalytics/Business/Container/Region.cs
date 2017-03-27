using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Container
{
    public class Region
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}
