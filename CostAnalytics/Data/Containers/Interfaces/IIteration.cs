using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers.Interfaces
{
    interface IIteration
    {
        int ID { get; set; }
        int RegionID { get; set; }
        int ProductID { get; set; }
        int ConfigurationID { get; set; }
        int SKUID { get; set; }
        int MeasureID { get; set; }
    }//end interface
}//end namespace
