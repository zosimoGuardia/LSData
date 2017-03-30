using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Containers.Interfaces
{
    interface IProduct
    {
        int ID { get; set; }
        string Name { get; set; }
        string LOB { get; set; }
        int Model { get; set; }
        string Variant { get; set; }
    }//end interface
}//end namespace
