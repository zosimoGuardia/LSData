using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers.Interfaces
{
    interface IProduct
    {
        int ID { get; set; }
        string Name { get; set; }
        string LOB { get; set; }
        string Model { get; set; }
        string Variant { get; set; }
        string Phase { get; set; }
    }//end interface
}//end namespace
