using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers.Interfaces
{
    interface IRegion
    {
        int ID { get; set; }
        string RegionName { get; set; }
        string Country { get; set; }
        string CountryCode { get; set; }
    }//end interface
}//end namespace
