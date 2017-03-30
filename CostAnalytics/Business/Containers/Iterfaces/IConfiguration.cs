using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface IConfiguration
    {
        int ID { get; set; }
        string Name { get; set; }
        string Type { get; set; }
    }//end interface
}//end namespace
