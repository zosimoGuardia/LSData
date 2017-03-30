using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Region:IContainer.IRegion
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}
