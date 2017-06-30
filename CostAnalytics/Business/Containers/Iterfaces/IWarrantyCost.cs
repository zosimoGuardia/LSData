using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Business.Containers;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface IWarrantyCost
    {
        int ID { get; set; }
        DateTime Date { get; set; }
        double Cost { get; set; }
        Cont.Warranty Warranty { get; set; }
        Cont.Configuration Configuration { get; set; }
    }//end interface
}//end namespace