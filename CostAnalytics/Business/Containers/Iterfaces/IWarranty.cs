using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Business.Containers;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface IWarranty
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }//end interface
}//end namespace