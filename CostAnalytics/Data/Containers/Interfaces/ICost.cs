using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers.Interfaces
{
    interface ICost
    {
        int ID { get; set; }
        int IterationID { get; set; }
        DateTime Date { get; set; }
        double CurrentCost { get; set; }
        double CostNext1 { get; set; }
        double CostNext2 { get; set; }
        double CostNext3 { get; set; }
    }//end interface
}//end namespace
