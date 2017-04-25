using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Business.Containers;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface IIteration
    {
        int ID { get; set; }
        Cont.Region Region { get; set; }
        Cont.Product Product { get; set; }
        Cont.Configuration Configuration { get; set; }
        Cont.SKU SKU { get; set; }
        Cont.Measure Measure { get; set; }
    }//end interface
}//end namespace
