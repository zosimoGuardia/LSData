using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface ISKU
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Commodity { get; set; }
    }//end interface
}//end namespace
