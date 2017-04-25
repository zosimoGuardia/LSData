using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface IMeasure
    {
        int ID { get; set; }
        string Name { get; set; }
    }//end interface
}//end namespace
